using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Kinect;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;

namespace SLRtry2
{
    class KinectSkeleton
    {
        KinectSensor sensor;
        static int colorCount = 0;
        static int depthCount = 0;
        static int lock1 = 0;
        private bool connected = false;
        public static int img_count = 0;
        public Bitmap depthImage;
        public Bitmap colorImage;

        public FingerCoordinates minx = new FingerCoordinates();
        public FingerCoordinates miny = new FingerCoordinates();
        public FingerCoordinates maxx = new FingerCoordinates();
        public FingerCoordinates maxy = new FingerCoordinates();

        private IntPtr depthPtr;
        private IntPtr colorPtr;

        private void skip() { }
        public delegate void afterReady();
        private afterReady afterColorReady;
        private afterReady afterDepthReady;
        public DepthImageFormat depthFormat { get; set; }
        public List<HandCoordinates> hands { get; set; }

        public KinectSkeleton()
        {
            depthFormat = DepthImageFormat.Resolution320x240Fps30;

            afterColorReady = skip;
            afterDepthReady = skip;
            hands = new List<HandCoordinates>();

            if (KinectSensor.KinectSensors.Count > 0)
            {
                connected = true;
                sensor = KinectSensor.KinectSensors.ElementAt(0);
                sensor.DepthFrameReady += new EventHandler<DepthImageFrameReadyEventArgs>(depthFrameReady);
                sensor.ColorFrameReady += new EventHandler<ColorImageFrameReadyEventArgs>(colorFrameReady);

            }
            else
            {
                connected = false;
            }

        }

        public void start()
        {
            sensor.DepthStream.Enable(depthFormat);
            sensor.ColorStream.Enable();
            sensor.Start();
        }

        public void stop()
        {
            sensor.DepthStream.Disable();
            sensor.ColorStream.Disable();
            sensor.Stop();
        }
        public void setEventColorReady(afterReady del)
        {
            afterColorReady = del;
        }

        public void clearEventColorReady()
        {
            afterColorReady = skip;
        }

        public void setEventDepthReady(afterReady del)
        {
            afterDepthReady = del;
        }

        public void clearEventDepthReady()
        {
            afterDepthReady = skip;
        }

        public void colorFrameReady(object sender, ColorImageFrameReadyEventArgs e)
        {
            using (ColorImageFrame frame = e.OpenColorImageFrame())
            {             
                if (frame == null)
                    return;
                colorCount++;

                if (colorCount % 10 == 0)
                {
                    byte[] pixels = new byte[frame.PixelDataLength];
                    frame.CopyPixelDataTo(pixels);

                    Marshal.FreeHGlobal(colorPtr);
                    colorPtr = Marshal.AllocHGlobal(pixels.Length);
                    Marshal.Copy(pixels, 0, colorPtr, pixels.Length);

                    int stride = frame.Width * 4;

                    colorImage = new Bitmap(
                        frame.Width,
                        frame.Height,
                        stride,
                        PixelFormat.Format32bppRgb,
                        colorPtr);

                    afterColorReady();
                }
                
            }
        }

        public void depthFrameReady(object sender, DepthImageFrameReadyEventArgs e)
        {
            using (DepthImageFrame frame = e.OpenDepthImageFrame())
            {
                if (frame == null)
                    return;
                depthCount++;

                if (depthCount % 10 == 0)
                {
                    int[] distances = generateDistances(frame);
                    bool[][] near = generateValidMatrix(frame, distances);
                    hands = localizeHands(near);

                    byte[] pixels = new byte[frame.PixelDataLength * 4];

                    // Free last depth Matrix
                    Marshal.FreeHGlobal(depthPtr);
                    depthPtr = Marshal.AllocHGlobal(pixels.Length);
                    Marshal.Copy(pixels, 0, depthPtr, pixels.Length);

                    // Create the bitmap
                    int height = near.Length;
                    int width = 0;
                    if (near.Length > 0)
                    {
                        width = near[0].Length;
                    }
                    int stride = width * 4;

                    depthImage = new Bitmap(
                        width,
                        height,
                        stride,
                        PixelFormat.Format32bppRgb,
                        depthPtr);

                    for (int i = 0; i < hands.Count; ++i)
                    {
                        hands[i].calculate3DPoints(320, 240, distances);
                    }

                    afterDepthReady();

                    // Draw fingertips and palm center
                    Graphics gBmp = Graphics.FromImage(depthImage);
                    Brush greenBrush = new SolidBrush(Color.Green);
                    Brush yellowBrush = new SolidBrush(Color.Yellow);
                    Brush redBrush = new SolidBrush(Color.Red);
                    Brush blueBrush = new SolidBrush(Color.Blue);
                    Brush violetBrush = new SolidBrush(Color.Orange);
                    for (int i = 0; i < hands.Count; ++i)
                    {
                        // Yellow point which is the center of the palm

                        gBmp.FillEllipse(yellowBrush, hands[i].palm.Y - 5, hands[i].palm.X - 5, 10, 10);
                        for (int j = 0; j < hands[i].contour.Count; ++j)
                        {
                            FingerCoordinates p = hands[i].contour[j];
                            depthImage.SetPixel(p.Y, p.X, Color.Snow);
                        }

                        // Blue points which represent the fingertips
                        for (int j = 0; j < hands[i].fingertips.Count; ++j)
                        {
                            if (hands[i].fingertips[j].X != -1)
                            {
                                gBmp.FillEllipse(greenBrush, hands[i].fingertips[j].Y - 5,
                                    hands[i].fingertips[j].X - 5, 10, 10);
                            }
                        }


                        minx.X = 4000;
                        miny.Y = 4000;
                        maxx.X = -1;
                        maxy.Y = -1;
                        for (int j = 0; j < hands[i].contour.Count; j++)
                        {
                            if (hands[i].contour[j].X < minx.X)
                            {
                                minx.X = hands[i].contour[j].X;
                                minx.Y = hands[i].contour[j].Y;
                            }
                            if (hands[i].contour[j].Y < miny.Y)
                            {
                                miny.X = hands[i].contour[j].X;
                                miny.Y = hands[i].contour[j].Y;
                            }
                            if (hands[i].contour[j].X > maxx.X)
                            {
                                maxx.X = hands[i].contour[j].X;
                                maxx.Y = hands[i].contour[j].Y;
                            }
                            if (hands[i].contour[j].Y > maxy.Y)
                            {
                                maxy.X = hands[i].contour[j].X;
                                maxy.Y = hands[i].contour[j].Y;
                            }
                        }
                        Rectangle rec = new Rectangle(miny.Y, minx.X, maxy.Y - miny.Y, maxx.X - minx.X);
                        Pen pen = new Pen(redBrush);
                        gBmp.DrawRectangle(pen, rec);
                        Pen penblue = new Pen(blueBrush);
                        gBmp.DrawLine(penblue, hands[i].palm.Y, hands[i].palm.X, miny.Y, hands[i].palm.X);
                        gBmp.DrawLine(penblue, hands[i].palm.Y, hands[i].palm.X, maxy.Y, hands[i].palm.X);
                        gBmp.DrawLine(penblue, hands[i].palm.Y, hands[i].palm.X, hands[i].palm.Y, minx.X);
                        gBmp.DrawLine(penblue, hands[i].palm.Y, hands[i].palm.X, hands[i].palm.Y, maxx.X);
                        Pen pen1 = new Pen(violetBrush);
                        for (int j = 0; j < hands[i].fingertips.Count; j++)
                        {
                            gBmp.DrawLine(pen1, hands[0].palm.Y, hands[0].palm.X, hands[i].fingertips[j].Y, hands[i].fingertips[j].X);
                            gBmp.DrawLine(pen1, hands[0].palm.Y, hands[0].palm.X, hands[0].palm.Y, minx.X);
                            gBmp.DrawLine(pen1, hands[i].fingertips[j].Y, hands[i].fingertips[j].X, hands[0].palm.Y, minx.X);

                        }
                    }
                    
                    violetBrush.Dispose();

                    greenBrush.Dispose();
                    yellowBrush.Dispose();
                    redBrush.Dispose();
                    blueBrush.Dispose();
                    gBmp.Dispose();
                }
                
            }
        }

        private int[] generateDistances(DepthImageFrame frame)
        {
            // Raw depth data form the Kinect
            short[] depth = new short[frame.PixelDataLength];
            frame.CopyPixelDataTo(depth);

            // Calculate the real distance
            int[] distance = new int[frame.PixelDataLength];
            for (int i = 0; i < distance.Length; ++i)
            {
                distance[i] = depth[i] >> DepthImageFrame.PlayerIndexBitmaskWidth;
            }

            return distance;
        }

        private bool[][] generateValidMatrix(DepthImageFrame frame, int[] distance)
        {
            // Create the matrix. The size depends on the margins
            int x1 = 0;
            int x2 = (int)(frame.Width);
            int y1 = 0;
            int y2 = (int)(frame.Height);
            bool[][] near = new bool[y2 - y1][];
            for (int i = 0; i < near.Length; ++i)
            {
                near[i] = new bool[x2 - x1];
            }

            // Calculate max and min distance
            int max = int.MinValue, min = int.MaxValue;

            for (int k = 0; k < distance.Length; ++k)
            {
                if (distance[k] > max) max = distance[k];
                if (distance[k] < min && distance[k] != -1) min = distance[k];
            }
            int margin = (int)(min + 0.05f * (max - min));
            int index = 0;
            for (int i = 0; i < near.Length; ++i)
            {
                for (int j = 0; j < near[i].Length; ++j)
                {
                    index = frame.Width * (i + y1) + (j + x1);
                    if (distance[index] <= margin && distance[index] != -1)
                    {
                        near[i][j] = true;
                    }
                    else
                    {
                        near[i][j] = false;
                    }
                }
            }
            int m;
            // First row
            for (int j = 0; j < near[0].Length; ++j)
                near[0][j] = false;

            // Last row
            m = near.Length - 1;
            for (int j = 0; j < near[0].Length; ++j)
                near[m][j] = false;

            // First column
            for (int i = 0; i < near.Length; ++i)
                near[i][0] = false;

            // Last column
            m = near[0].Length - 1;
            for (int i = 0; i < near.Length; ++i)
                near[i][m] = false;

            return near;
        }

        private List<HandCoordinates> localizeHands(bool[][] valid)
        {
            int i, j, k;

            List<HandCoordinates> hands = new List<HandCoordinates>();

            List<FingerCoordinates> insidePoints = new List<FingerCoordinates>();
            List<FingerCoordinates> contourPoints = new List<FingerCoordinates>();


            bool[][] contour = new bool[valid.Length][];
            for (i = 0; i < valid.Length; ++i)
            {
                contour[i] = new bool[valid[0].Length];
            }

            // Divide points in contour and inside points
            int count = 0;
            for (i = 1; i < valid.Length - 1; ++i)
            {
                for (j = 1; j < valid[i].Length - 1; ++j)
                {

                    if (valid[i][j])
                    {
                        // Count the number of valid adjacent points
                        count = this.numValidPixelAdjacent(ref i, ref j, ref valid);

                        if (count == 4) // Inside
                        {
                            insidePoints.Add(new FingerCoordinates(i, j));
                        }
                        else // Contour
                        {
                            contour[i][j] = true;
                            contourPoints.Add(new FingerCoordinates(i, j));
                        }

                    }
                }
            }

            for (i = 0; i < contourPoints.Count; ++i)
            {
                HandCoordinates hand = new HandCoordinates();
                if (contour[contourPoints[i].X][contourPoints[i].Y])
                {
                    hand.contour = CalculateFrontier(ref valid, contourPoints[i], ref contour);
                    if (hand.contour.Count / (contourPoints.Count * 1.0f) > 0.20f
                        && hand.contour.Count > 22)
                    {
                        hand.calculateContainerBox(0.5f);
                        hands.Add(hand);
                    }
                    if (hands.Count >= 2)
                    {
                        break;
                    }
                }

            }
            for (i = 0; i < insidePoints.Count; ++i)
            {
                for (j = 0; j < hands.Count; ++j)
                {
                    if (hands[j].isPointInsideContainerBox(insidePoints[i]))
                    {
                        hands[j].inside.Add(insidePoints[i]);
                    }
                }
                
            }

            // Find the center of the palm
            float min, max, distance = 0;

            for (i = 0; i < hands.Count; ++i)
            {
                max = float.MinValue;
                for (j = 0; j < hands[i].inside.Count; j += 8)
                {
                    min = float.MaxValue;
                    for (k = 0; k < hands[i].contour.Count; k += 8)
                    {
                        distance = FingerCoordinates.distanceEuclidean(hands[i].inside[j], hands[i].contour[k]);
                        if (!hands[i].isCircleInsideContainerBox(hands[i].inside[j], distance)) continue;
                        if (distance < min) min = distance;
                        if (min < max) break;
                    }

                    if (max < min && min != float.MaxValue)
                    {
                        max = min;
                        hands[i].palm = hands[i].inside[j];
                    }
                }
            }

            // Find the fingertips
            FingerCoordinates p1, p2, p3, pAux, r1, r2;
            int size;
            double angle;
            int jump;

            for (i = 0; i < hands.Count; ++i)
            {
                max = hands[i].contour.Count;
                size = hands[i].contour.Count;
                jump = (int)(size * 0.10f);
                for (j = 0; j < 22; j += 1)
                {
                    p1 = hands[i].contour[(j - 22 + size) % size];
                    p2 = hands[i].contour[j];
                    p3 = hands[i].contour[(j + 22) % size];
                    r1 = p1 - p2;
                    r2 = p3 - p2;

                    angle = FingerCoordinates.angle(r1, r2);

                    if (angle > 0 && angle < (40 * (Math.PI / 180)))
                    {
                        pAux = p3 + ((p1 - p3) / 2);
                        if (FingerCoordinates.distanceEuclideanSquared(pAux, hands[i].palm) >
                            FingerCoordinates.distanceEuclideanSquared(hands[i].contour[j], hands[i].palm))
                            continue;

                        hands[i].fingertips.Add(hands[i].contour[j]);
                        max = hands[i].contour.Count + j - jump;
                        max = Math.Min(max, hands[i].contour.Count);
                        j += jump;
                        break;
                    }
                }

                // Continue with the rest of the points
                for (; j < max; j += 2)
                {
                    p1 = hands[i].contour[(j - 22 + size) % size];
                    p2 = hands[i].contour[j];
                    p3 = hands[i].contour[(j + 22) % size];
                    r1 = p1 - p2;
                    r2 = p3 - p2;

                    angle = FingerCoordinates.angle(r1, r2);

                    if (angle > 0 && angle < (40 * (Math.PI / 180)))
                    {
                        pAux = p3 + ((p1 - p3) / 2);
                        if (FingerCoordinates.distanceEuclideanSquared(pAux, hands[i].palm) >
                            FingerCoordinates.distanceEuclideanSquared(hands[i].contour[j], hands[i].palm))
                            continue;

                        hands[i].fingertips.Add(hands[i].contour[j]);
                        j += jump;
                    }
                }
            }

            return hands;
        }
        
        private List<FingerCoordinates> CalculateFrontier(ref bool[][] valid, FingerCoordinates start, ref bool[][] contour)
        {
            List<FingerCoordinates> list = new List<FingerCoordinates>();
            FingerCoordinates last = new FingerCoordinates(-1, -1);
            FingerCoordinates current = new FingerCoordinates(start);
            int dir = 0;

            do
            {
                if (valid[current.X][current.Y])
                {
                    dir = (dir + 1) % 4;
                    if (current != last)
                    {
                        list.Add(new FingerCoordinates(current.X, current.Y));
                        last = new FingerCoordinates(current);
                        contour[current.X][current.Y] = false;
                    }
                }
                else
                {
                    dir = (dir + 4 - 1) % 4;
                }

                switch (dir)
                {
                    case 0: current.X += 1; break; // Down
                    case 1: current.Y += 1; break; // Right
                    case 2: current.X -= 1; break; // Up
                    case 3: current.Y -= 1; break; // Left
                }
            } while (current != start);

            return list;
        }
        
        private int numValidPixelAdjacent(ref int i, ref int j, ref bool[][] valid)
        {
            int count = 0;

            if (valid[i + 1][j]) ++count;
            if (valid[i - 1][j]) ++count;
            if (valid[i][j + 1]) ++count;
            if (valid[i][j - 1]) ++count;
            return count;
        }

        public bool isConnected()
        {
            return connected;
        }

        public Bitmap getDepthImage()
        {
            return depthImage;
        }

        public Bitmap getColorImage()
        {
            return colorImage;
        }

    }
}
