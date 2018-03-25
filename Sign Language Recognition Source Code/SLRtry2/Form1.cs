using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;
using Microsoft.Kinect;

namespace SLRtry2
{
    public partial class Form1 : Form
    {
        KinectSkeleton kinectController;
        public Form1()
        {
            InitializeComponent();
            kinectController = new KinectSkeleton();

            kinectController.setEventColorReady(drawColorImage);

            if (kinectController.isConnected())
            {
                kinectController.start();
            }
            else
            {
                // Show an error
                Console.WriteLine("Kinect device not available");
            }

        }

        // Show the color image in both image elements
        private void drawColorImage()
        {
            RGBPicture.Image = kinectController.getColorImage();
            DepthPicture.Image = kinectController.getColorImage();
        }

        // Show the color and tracked images in the image elements
        private void drawDepthImage()
        {
            RGBPicture.Image = kinectController.getColorImage();
            DepthPicture.Image = kinectController.getDepthImage();
        }

        private double angle_vertical_axis(int finger_x, int finger_y)
        {
            double a2, b2, c2, a, b, c;
            a2 = Math.Pow((kinectController.hands[0].palm.X - finger_x), 2) + Math.Pow((kinectController.hands[0].palm.Y - finger_y), 2);
            c2 = Math.Pow((kinectController.hands[0].palm.X - kinectController.minx.X), 2);
            b2 = Math.Pow((finger_x - kinectController.minx.X), 2) + Math.Pow((finger_y - kinectController.hands[0].palm.Y), 2);
            a = Math.Sqrt(a2);
            c = Math.Sqrt(c2);
            b = Math.Sqrt(b2);
            Console.WriteLine("a " + a + " b " + b + " c " + c);
            double cosb;
            cosb = (a2 + c2 - b2) / (2 * a * c);
            double radb, degb;
            radb = Math.Acos(cosb);
            degb = radb * 180 / Math.PI;
            if (degb > 90)
            {
                degb = 180 - degb;
            }
            Console.WriteLine("angle degb " + degb);

            return degb;
        }
        private void showNumberFingers()
        {
            ThreadPool.QueueUserWorkItem(delegate
            {
                if (kinectController.hands.Count > 0)
                {
                    if (kinectController.hands[0].fingertips.Count == 0 && kinectController.hands[1].fingertips.Count == 0)
                    {
                        Invoke(new Action(delegate { SignTextBox.Text = "0"; }));
                    }
                    else if (kinectController.hands[0].fingertips.Count > 0 && kinectController.hands[1].fingertips.Count > 0)
                    {
                        if (kinectController.hands[0].fingertips.Count == 5)
                        {
                            if (kinectController.hands[1].fingertips.Count == 1)
                            {
                                Invoke(new Action(delegate { SignTextBox.Text = "6"; }));
                            }
                            else if (kinectController.hands[1].fingertips.Count == 2)
                            {
                                Invoke(new Action(delegate { SignTextBox.Text = "7"; }));
                            }
                            else if (kinectController.hands[1].fingertips.Count == 3)
                            {
                                Invoke(new Action(delegate { SignTextBox.Text = "8"; }));
                            }
                            else if (kinectController.hands[1].fingertips.Count == 4)
                            {
                                Invoke(new Action(delegate { SignTextBox.Text = "9"; }));
                            }
                            else if (kinectController.hands[1].fingertips.Count == 5)
                            {
                                Invoke(new Action(delegate { SignTextBox.Text = "10"; }));
                            }
                        } else if (kinectController.hands[1].fingertips.Count == 5) {
                            if (kinectController.hands[0].fingertips.Count == 1)
                            {
                                Invoke(new Action(delegate { SignTextBox.Text = "6"; }));
                            }
                            else if (kinectController.hands[0].fingertips.Count == 2)
                            {
                                Invoke(new Action(delegate { SignTextBox.Text = "7"; }));
                            }
                            else if (kinectController.hands[0].fingertips.Count == 3)
                            {
                                Invoke(new Action(delegate { SignTextBox.Text = "8"; }));
                            }
                            else if (kinectController.hands[0].fingertips.Count == 4)
                            {
                                Invoke(new Action(delegate { SignTextBox.Text = "9"; }));
                            }
                            else if (kinectController.hands[0].fingertips.Count == 5)
                            {
                                Invoke(new Action(delegate { SignTextBox.Text = "10"; }));
                            }
                        }
                    } else if (kinectController.hands[0].fingertips.Count > 0 && kinectController.hands[1].fingertips.Count == 0) {
                        if (kinectController.hands[0].fingertips.Count == 5)
                        {
                            if (kinectController.hands[1].fingertips.Count == 0)
                            {
                                Invoke(new Action(delegate { SignTextBox.Text = "5"; }));
                            }
                        }
                        if (kinectController.hands[0].fingertips.Count == 1)
                        {
                            // Invoke(new Action(delegate { SentenceTextBox2.Text = "Sign with 1 finger"; }));
                            double angle = angle_vertical_axis(kinectController.hands[0].fingertips[0].X, kinectController.hands[0].fingertips[0].Y);

                            if (angle >= 10 && angle <= 21 && kinectController.hands[0].fingertips[0].Y < kinectController.hands[0].palm.Y)
                            {
                                Invoke(new Action(delegate { SignTextBox.Text = "1"; }));
                            }
                            else if (angle >= 10 && angle <= 23 && kinectController.hands[0].fingertips[0].Y > kinectController.hands[0].palm.Y)
                            {
                                Invoke(new Action(delegate { SignTextBox.Text = "I"; }));
                            }
                            else if (angle >= 46 && angle <= 65)
                            {
                                Invoke(new Action(delegate { SignTextBox.Text = "A"; }));
                            }
                            else if (angle >= 22 && angle <= 45 && kinectController.hands[0].fingertips[0].Y < kinectController.hands[0].palm.Y)
                            {
                                Invoke(new Action(delegate { SignTextBox.Text = "X"; }));
                            }
                            else if (angle >= 0 && angle <= 9)
                            {
                                Invoke(new Action(delegate { SignTextBox.Text = "D"; }));
                            }
                            else if (angle >= 66 && angle <= 85)
                            {
                                Invoke(new Action(delegate { SignTextBox.Text = "G"; }));
                            }

                            //Invoke(new Action(delegate { SentenceTextBox2.Text = angle.ToString(); }));
                            Console.WriteLine("1 finger " + (int)(kinectController.hands[0].fingertips[0].X - kinectController.hands[0].palm.X) + " " + (int)(kinectController.hands[0].fingertips[0].Y - kinectController.hands[0].palm.Y));

                        }
                        else if (kinectController.hands[0].fingertips.Count == 2)
                        {
                            double angle1, angle2;
                            angle1 = angle_vertical_axis(kinectController.hands[0].fingertips[0].X, kinectController.hands[0].fingertips[0].Y);
                            angle2 = angle_vertical_axis(kinectController.hands[0].fingertips[1].X, kinectController.hands[0].fingertips[1].Y);
                            Console.WriteLine("angle 1 : " + angle1.ToString() + " " + "angle 2 : " + angle2.ToString()  );

                            int px = kinectController.hands[0].palm.X;
                            int py = kinectController.hands[0].palm.Y;
                            int fx0 = kinectController.hands[0].fingertips[0].X;
                            int fx1 = kinectController.hands[0].fingertips[1].X;
                            int fy0 = kinectController.hands[0].fingertips[0].Y;
                            int fy1 = kinectController.hands[0].fingertips[1].Y;
                            
                            //Invoke(new Action(delegate { SentenceTextBox2.Text = "angle 1 : " + angle1.ToString() + " " + "angle 2 : " + angle2.ToString() + "palm x" + px.ToString() + "palm y" + py.ToString() +  "finger f1x" + fx0.ToString() + "finger f2x" + fx1.ToString() + "finger f1y" + fy0.ToString() + "finger f2y" + fy1.ToString(); }));

                           // Invoke(new Action(delegate { SentenceTextBox2.Text = ""; }));

                            Console.WriteLine("X coordinate finger 1" + kinectController.hands[0].fingertips[0].X);
                            Console.WriteLine("Y Coordinate finger 1" + kinectController.hands[0].fingertips[0].Y);
                            Console.WriteLine("X coordinate finger 2" + kinectController.hands[0].fingertips[1].X);
                            Console.WriteLine("Y Coordinate finger 2" + kinectController.hands[0].fingertips[1].Y);
                            Console.WriteLine("X coordinate palm " + kinectController.hands[0].palm.X);
                            Console.WriteLine("Y Coordinate palm " + kinectController.hands[0].palm.Y);
                            Console.WriteLine("X diff palm f1" + (kinectController.hands[0].palm.X - kinectController.hands[0].fingertips[0].X));
                            Console.WriteLine("Y diff palm f1" + (kinectController.hands[0].palm.Y - kinectController.hands[0].fingertips[0].Y));

                            Console.WriteLine("X diff palm f2" + (kinectController.hands[0].palm.X - kinectController.hands[0].fingertips[1].X));
                            Console.WriteLine("Y diff palm f2" + (kinectController.hands[0].palm.Y - kinectController.hands[0].fingertips[1].Y));

                            Console.WriteLine("X diff finger" + (kinectController.hands[0].fingertips[0].X - kinectController.hands[0].fingertips[1].X));
                            
                            if ((angle1 >= 0 && angle1 <= 50 && angle2 >= 60 && angle2 <= 80 && (kinectController.hands[0].fingertips[0].X > kinectController.hands[0].palm.X) && (kinectController.hands[0].fingertips[1].X > kinectController.hands[0].palm.X)) || (angle2 >= 0 && angle2 <= 50 && angle1 >= 60 && angle1 <= 80 && (kinectController.hands[0].fingertips[0].X > kinectController.hands[0].palm.X) && (kinectController.hands[0].fingertips[1].X > kinectController.hands[0].palm.X)))
                            {
                                Invoke(new Action(delegate { SignTextBox.Text = "Q"; }));
                            }
                            else if ((angle1 >= 80 && angle1 <= 90 && angle2 >= 60 && angle2 <= 80 && ((kinectController.hands[0].fingertips[0].Y < kinectController.hands[0].palm.Y && kinectController.hands[0].fingertips[1].Y < kinectController.hands[0].palm.Y) || (kinectController.hands[0].fingertips[1].Y > kinectController.hands[0].palm.Y && kinectController.hands[0].fingertips[0].Y > kinectController.hands[0].palm.Y))) || (angle2 >= 80 && angle2 <= 90 && angle1 >= 60 && angle1 <= 80 && ((kinectController.hands[0].fingertips[0].Y < kinectController.hands[0].palm.Y && kinectController.hands[0].fingertips[1].Y < kinectController.hands[0].palm.Y) || (kinectController.hands[0].fingertips[1].Y > kinectController.hands[0].palm.Y && kinectController.hands[0].fingertips[0].Y > kinectController.hands[0].palm.Y))))
                            {
                                Invoke(new Action(delegate { SignTextBox.Text = "H"; }));
                            }
                            else if ((angle1 >= 8 && angle1 <= 20 && angle2 >= 0 && angle2 <= 10) || (angle2 >= 8 && angle2 <= 20 && angle1 >= 0 && angle1 <= 10))
                            {
                                Invoke(new Action(delegate { SignTextBox.Text = "U"; }));
                            }
                            else if ((angle1 >= 15 && angle1 <= 25 && angle2 >= 11 && angle2 <= 28) || (angle2 >= 15 && angle2 <= 25 && angle1 >= 11 && angle1 <= 28))
                            {
                                Invoke(new Action(delegate { SignTextBox.Text = "V"; }));
                            }
                            else if ((angle1 >= 35 && angle1 <= 65 && angle2 >= 50 && angle2 <= 80 && ((kinectController.hands[0].fingertips[0].Y < kinectController.hands[0].palm.Y && kinectController.hands[0].fingertips[1].Y > kinectController.hands[0].palm.Y) || (kinectController.hands[0].fingertips[1].Y < kinectController.hands[0].palm.Y && kinectController.hands[0].fingertips[0].Y > kinectController.hands[0].palm.Y))) || (angle2 >= 35 && angle2 <= 65 && angle1 >= 50 && angle1 <= 80 && ((kinectController.hands[0].fingertips[0].Y < kinectController.hands[0].palm.Y && kinectController.hands[0].fingertips[1].Y > kinectController.hands[0].palm.Y) || (kinectController.hands[0].fingertips[1].Y < kinectController.hands[0].palm.Y && kinectController.hands[0].fingertips[0].Y > kinectController.hands[0].palm.Y))))
                            {
                                Invoke(new Action(delegate { SignTextBox.Text = "Y"; }));
                            }
                            else if ((angle1 >= 0 && angle1 <= 25 && angle2 >= 35 && angle2 <= 55 && (kinectController.hands[0].fingertips[0].X < kinectController.hands[0].palm.X) && (kinectController.hands[0].fingertips[1].X < kinectController.hands[0].palm.X)) || (angle2 >= 0 && angle2 <= 25 && angle1 >= 35 && angle1 <= 55 && (kinectController.hands[0].fingertips[0].X < kinectController.hands[0].palm.X) && (kinectController.hands[0].fingertips[1].X < kinectController.hands[0].palm.X)))
                            {
                                Invoke(new Action(delegate { SignTextBox.Text = "K"; }));
                            }
                            else if ((kinectController.hands[0].fingertips[0].X < kinectController.hands[0].palm.X) && (kinectController.hands[0].fingertips[1].X < kinectController.hands[0].palm.X))
                            {
                                Invoke(new Action(delegate { SignTextBox.Text = "2"; }));
                            }
                            /*else if ((angle1 >= 0 && angle1 <= 25 && angle2 >= 35 && angle2 <= 55 && (kinectController.hands[0].fingertips[0].X > kinectController.hands[0].palm.X) && (kinectController.hands[0].fingertips[1].X > kinectController.hands[0].palm.X)) || (angle2 >= 0 && angle2 <= 25 && angle1 >= 35 && angle1 <= 55 && (kinectController.hands[0].fingertips[0].X > kinectController.hands[0].palm.X) && (kinectController.hands[0].fingertips[1].X > kinectController.hands[0].palm.X)))
                            {
                                Invoke(new Action(delegate { SignTextBox.Text = "Q"; }));
                            }*/                            
                            else if ((angle1 >= 35 && angle1 <= 65 && angle2 >= 50 && angle2 <= 70 && ((kinectController.hands[0].fingertips[0].Y < kinectController.hands[0].palm.Y && kinectController.hands[0].fingertips[1].Y < kinectController.hands[0].palm.Y) || (kinectController.hands[0].fingertips[1].Y > kinectController.hands[0].palm.Y && kinectController.hands[0].fingertips[0].Y > kinectController.hands[0].palm.Y))) || (angle2 >= 35 && angle2 <= 65 && angle1 >= 50 && angle1 <= 70 && ((kinectController.hands[0].fingertips[0].Y < kinectController.hands[0].palm.Y && kinectController.hands[0].fingertips[1].Y < kinectController.hands[0].palm.Y) || (kinectController.hands[0].fingertips[1].Y > kinectController.hands[0].palm.Y && kinectController.hands[0].fingertips[0].Y > kinectController.hands[0].palm.Y))))
                            {
                                Invoke(new Action(delegate { SignTextBox.Text = "C"; }));
                            }
                            else if ((angle1 >= 11 && angle1 <= 30 && angle2 >= 70 && angle2 <= 90 && (kinectController.hands[0].fingertips[0].X < kinectController.hands[0].palm.X)) || (angle2 >= 11 && angle2 <= 30 && angle1 >= 70 && angle1 <= 90 && (kinectController.hands[0].fingertips[1].X < kinectController.hands[0].palm.X)))
                            {
                                Invoke(new Action(delegate { SignTextBox.Text = "L"; }));
                            }
                            else if ((angle1 >= 11 && angle1 <= 30 && angle2 >= 70 && angle2 <= 90 && (kinectController.hands[0].fingertips[0].X > kinectController.hands[0].palm.X)) || (angle2 >= 11 && angle2 <= 30 && angle1 >= 70 && angle1 <= 90 && (kinectController.hands[0].fingertips[1].X > kinectController.hands[0].palm.X)))
                            {
                                Invoke(new Action(delegate { SignTextBox.Text = "P"; }));
                            }

                            Console.WriteLine("Y diff finger" + (kinectController.hands[0].fingertips[0].Y - kinectController.hands[0].fingertips[1].Y));
                        }

                        else if (kinectController.hands[0].fingertips.Count == 3)
                        {
                            double angle1, angle2, angle3;
                            angle1 = angle_vertical_axis(kinectController.hands[0].fingertips[0].X, kinectController.hands[0].fingertips[0].Y);
                            angle2 = angle_vertical_axis(kinectController.hands[0].fingertips[1].X, kinectController.hands[0].fingertips[1].Y);
                            angle3 = angle_vertical_axis(kinectController.hands[0].fingertips[2].X, kinectController.hands[0].fingertips[2].Y);
                            Console.WriteLine("angle 1 : " + angle1.ToString() + " " + "angle 2 : " + angle2.ToString() + " angle 3 : " + angle3.ToString());
                            //Invoke(new Action(delegate { SentenceTextBox2.Text = "angle 1 : " + angle1.ToString() + " " + "angle 2 : " + angle2.ToString() + " angle 3 : " + angle3.ToString(); }));

                            if ((angle1 >= 0 && angle1 <= 10 && angle2 >= 70 && angle2 <= 90 && angle3 >= 30 && angle3 <= 55) || (angle1 >= 0 && angle1 <= 10 && angle3 >= 70 && angle3 <= 90 && angle2 >= 30 && angle2 <= 55) || (angle3 >= 0 && angle3 <= 10 && angle2 >= 70 && angle2 <= 90 && angle1 >= 30 && angle1 <= 55) || 
                            (angle2 >= 0 && angle2 <= 10 && angle1 >= 70 && angle1 <= 90 && angle3 >= 30 && angle3 <= 55) || (angle2 >= 0 && angle2 <= 10 && angle3 >= 70 && angle3 <= 90 && angle1 >= 30 && angle1 <= 55) || (angle3 >= 0 && angle3 <= 10 && angle1 >= 70 && angle1 <= 90 && angle2 >= 30 && angle2 <= 55))
                            {
                                Invoke(new Action(delegate { SignTextBox.Text = "3"; }));
                            }
                            else if ((angle1 >= 0 && angle1 <= 10 && angle2 >= 50 && angle2 <= 69 && angle3 >= 20 && angle3 <= 45) || (angle1 >= 0 && angle1 <= 10 && angle3 >= 50 && angle3 <= 69 && angle2 >= 20 && angle2 <= 45) || (angle3 >= 0 && angle3 <= 10 && angle2 >= 50 && angle2 <= 69 && angle1 >= 20 && angle1 <= 45) ||
                            (angle2 >= 0 && angle2 <= 10 && angle1 >= 50 && angle1 <= 69 && angle3 >= 25 && angle3 <= 45) || (angle2 >= 0 && angle2 <= 10 && angle3 >= 50 && angle3 <= 69 && angle1 >= 20 && angle1 <= 45) || (angle3 >= 0 && angle3 <= 10 && angle1 >= 50 && angle1 <= 69 && angle2 >= 20 && angle2 <= 45))
                            {
                                Invoke(new Action(delegate { SignTextBox.Text = "F"; }));
                            }
                            else if ((angle1 >= 0 && angle1 <= 10 && angle2 >= 20 && angle2 <= 35 && angle3 >= 25 && angle3 <= 40) || (angle1 >= 0 && angle1 <= 10 && angle3 >= 20 && angle3 <= 35 && angle2 >= 25 && angle2 <= 40) || (angle3 >= 0 && angle3 <= 10 && angle2 >= 20 && angle2 <= 35 && angle1 >= 25 && angle1 <= 40) ||
                            (angle2 >= 0 && angle2 <= 10 && angle1 >= 20 && angle1 <= 35 && angle3 >= 25 && angle3 <= 40) || (angle2 >= 0 && angle2 <= 10 && angle3 >= 20 && angle3 <= 35 && angle1 >= 25 && angle1 <= 40) || (angle3 >= 0 && angle3 <= 10 && angle1 >= 20 && angle1 <= 35 && angle2 >= 25 && angle2 <= 40))
                            {
                                Invoke(new Action(delegate { SignTextBox.Text = "W"; }));
                            }

                            //if (((angle1 >= 4 || angle2 >= 4 || angle3 >= 4) && (angle1 <= 10 || angle2 <= 10 || angle3 <= 10)) && (angle1 >= 65 || angle2 >= 65 || angle3 >= 65) && (angle1 <= 87 || angle2 <= 87 || angle3 <= 87) && (angle1 >= 15 || angle2 >= 15 || angle3 >= 15) && (angle1 <= 40 || angle2 <= 40 || angle3 <= 40))



                            /*else if ((angle1 >= 1 || angle2 >= 1 || angle3 >= 1) && (angle1 <= 9 || angle2 <= 9 || angle3 <= 9) && (angle1 >= 13 || angle2 >= 13 || angle3 >= 13) && (angle1 <= 23 || angle2 <= 23 || angle3 <= 23) && (angle1 >= 33 || angle2 >= 33 || angle3 >= 33) && (angle1 <= 51 || angle2 <= 51 || angle3 <= 51))
                            {
                                Invoke(new Action(delegate { SignTextBox.Text = "W"; }));
                            }
                            else if ((angle1 >= 1 || angle2 >= 1 || angle3 >= 1) && (angle1 <= 9 || angle2 <= 9 || angle3 <= 9) && (angle1 >= 20 || angle2 >= 20 || angle3 >= 20) && (angle1 <= 30 || angle2 <= 30 || angle3 <= 30) && (angle1 >= 20 || angle2 >= 20 || angle3 >= 20) && (angle1 <= 30 || angle2 <= 30 || angle3 <= 30))
                            {
                                Invoke(new Action(delegate { SignTextBox.Text = "F"; }));
                            }*/

                        }
                        else if (kinectController.hands[0].fingertips.Count == 4)
                        {
                            Invoke(new Action(delegate { SignTextBox.Text = "4"; }));
                        }

                    }
                } else if (kinectController.hands[1].fingertips.Count > 0 && kinectController.hands[0].fingertips.Count == 0)
                {
                    if (kinectController.hands[1].fingertips.Count == 5)
                    {
                        if (kinectController.hands[0].fingertips.Count == 0)
                        {
                            Invoke(new Action(delegate { SignTextBox.Text = "5"; }));
                        }
                    }
                    if (kinectController.hands[1].fingertips.Count == 1)
                    {
                        // Invoke(new Action(delegate { SentenceTextBox2.Text = "Sign with 1 finger"; }));
                        double angle = angle_vertical_axis(kinectController.hands[1].fingertips[0].X, kinectController.hands[1].fingertips[0].Y);

                        if (angle >= 10 && angle <= 21 && kinectController.hands[1].fingertips[0].Y < kinectController.hands[1].palm.Y)
                        {
                            Invoke(new Action(delegate { SignTextBox.Text = "1"; }));
                        }
                        else if (angle >= 10 && angle <= 23 && kinectController.hands[1].fingertips[0].Y > kinectController.hands[1].palm.Y)
                        {
                            Invoke(new Action(delegate { SignTextBox.Text = "I"; }));
                        }
                        else if (angle >= 46 && angle <= 65)
                        {
                            Invoke(new Action(delegate { SignTextBox.Text = "A"; }));
                        }
                        else if (angle >= 22 && angle <= 45)
                        {
                            Invoke(new Action(delegate { SignTextBox.Text = "X"; }));
                        }
                        else if (angle >= 0 && angle <= 9)
                        {
                            Invoke(new Action(delegate { SignTextBox.Text = "D"; }));
                        }
                        else if (angle >= 66 && angle <= 85)
                        {
                            Invoke(new Action(delegate { SignTextBox.Text = "G"; }));
                        }

                        //Invoke(new Action(delegate { SentenceTextBox2.Text = angle.ToString(); }));
                        Console.WriteLine("1 finger " + (int)(kinectController.hands[1].fingertips[0].X - kinectController.hands[1].palm.X) + " " + (int)(kinectController.hands[1].fingertips[0].Y - kinectController.hands[1].palm.Y));

                    }
                    else if (kinectController.hands[1].fingertips.Count == 2)
                    {
                        double angle1, angle2;
                        angle1 = angle_vertical_axis(kinectController.hands[1].fingertips[0].X, kinectController.hands[1].fingertips[0].Y);
                        angle2 = angle_vertical_axis(kinectController.hands[1].fingertips[1].X, kinectController.hands[1].fingertips[1].Y);
                        Console.WriteLine("angle 1 : " + angle1.ToString() + " " + "angle 2 : " + angle2.ToString());

                        int px = kinectController.hands[1].palm.X;
                        int py = kinectController.hands[1].palm.Y;
                        int fx0 = kinectController.hands[1].fingertips[0].X;
                        int fx1 = kinectController.hands[1].fingertips[1].X;
                        int fy0 = kinectController.hands[1].fingertips[0].Y;
                        int fy1 = kinectController.hands[1].fingertips[1].Y;

                       // Invoke(new Action(delegate { SentenceTextBox2.Text = "angle 1 : " + angle1.ToString() + " " + "angle 2 : " + angle2.ToString() + "palm x" + px.ToString() + "palm y" + py.ToString() + "finger f1x" + fx0.ToString() + "finger f2x" + fx1.ToString() + "finger f1y" + fy0.ToString() + "finger f2y" + fy1.ToString(); }));

                        // Invoke(new Action(delegate { SentenceTextBox2.Text = ""; }));

                        Console.WriteLine("X coordinate finger 1" + kinectController.hands[1].fingertips[0].X);
                        Console.WriteLine("Y Coordinate finger 1" + kinectController.hands[1].fingertips[0].Y);
                        Console.WriteLine("X coordinate finger 2" + kinectController.hands[1].fingertips[1].X);
                        Console.WriteLine("Y Coordinate finger 2" + kinectController.hands[1].fingertips[1].Y);
                        Console.WriteLine("X coordinate palm " + kinectController.hands[1].palm.X);
                        Console.WriteLine("Y Coordinate palm " + kinectController.hands[1].palm.Y);
                        Console.WriteLine("X diff palm f1" + (kinectController.hands[1].palm.X - kinectController.hands[1].fingertips[0].X));
                        Console.WriteLine("Y diff palm f1" + (kinectController.hands[1].palm.Y - kinectController.hands[1].fingertips[0].Y));

                        Console.WriteLine("X diff palm f2" + (kinectController.hands[1].palm.X - kinectController.hands[1].fingertips[1].X));
                        Console.WriteLine("Y diff palm f2" + (kinectController.hands[1].palm.Y - kinectController.hands[1].fingertips[1].Y));

                        Console.WriteLine("X diff finger" + (kinectController.hands[1].fingertips[0].X - kinectController.hands[1].fingertips[1].X));

                        if ((angle1 >= 0 && angle1 <= 50 && angle2 >= 60 && angle2 <= 80 && (kinectController.hands[1].fingertips[0].X > kinectController.hands[1].palm.X) && (kinectController.hands[1].fingertips[1].X > kinectController.hands[1].palm.X)) || (angle2 >= 0 && angle2 <= 50 && angle1 >= 60 && angle1 <= 80 && (kinectController.hands[1].fingertips[0].X > kinectController.hands[1].palm.X) && (kinectController.hands[1].fingertips[1].X > kinectController.hands[1].palm.X)))
                        {
                            Invoke(new Action(delegate { SignTextBox.Text = "Q"; }));
                        } else if ((angle1 >= 80 && angle1 <= 90 && angle2 >= 60 && angle2 <= 80 && (kinectController.hands[1].fingertips[0].X > kinectController.hands[1].palm.X) && (kinectController.hands[1].fingertips[1].X > kinectController.hands[1].palm.X)) || (angle2 >= 80 && angle2 <= 90 && angle1 >= 60 && angle1 <= 80 && (kinectController.hands[1].fingertips[0].X > kinectController.hands[1].palm.X) && (kinectController.hands[1].fingertips[1].X > kinectController.hands[1].palm.X)))
                        {
                            Invoke(new Action(delegate { SignTextBox.Text = "H"; }));
                        }
                        else if ((angle1 >= 8 && angle1 <= 20 && angle2 >= 0 && angle2 <= 10) || (angle2 >= 8 && angle2 <= 20 && angle1 >= 0 && angle1 <= 10))
                        {
                            Invoke(new Action(delegate { SignTextBox.Text = "U"; }));
                        }
                        else if ((angle1 >= 15 && angle1 <= 25 && angle2 >= 11 && angle2 <= 28) || (angle2 >= 15 && angle2 <= 25 && angle1 >= 11 && angle1 <= 28))
                        {
                            Invoke(new Action(delegate { SignTextBox.Text = "V"; }));
                        }
                        else if ((angle1 >= 35 && angle1 <= 65 && angle2 >= 50 && angle2 <= 80 && ((kinectController.hands[1].fingertips[0].Y < kinectController.hands[1].palm.Y && kinectController.hands[1].fingertips[1].Y > kinectController.hands[1].palm.Y) || (kinectController.hands[1].fingertips[1].Y < kinectController.hands[1].palm.Y && kinectController.hands[1].fingertips[0].Y > kinectController.hands[1].palm.Y))) || (angle2 >= 35 && angle2 <= 65 && angle1 >= 50 && angle1 <= 80 && ((kinectController.hands[1].fingertips[0].Y < kinectController.hands[1].palm.Y && kinectController.hands[1].fingertips[1].Y > kinectController.hands[1].palm.Y) || (kinectController.hands[1].fingertips[1].Y < kinectController.hands[1].palm.Y && kinectController.hands[1].fingertips[0].Y > kinectController.hands[1].palm.Y))))
                        {
                            Invoke(new Action(delegate { SignTextBox.Text = "Y"; }));
                        }
                        else if ((angle1 >= 0 && angle1 <= 25 && angle2 >= 35 && angle2 <= 55 && (kinectController.hands[1].fingertips[0].X < kinectController.hands[1].palm.X) && (kinectController.hands[1].fingertips[1].X < kinectController.hands[1].palm.X)) || (angle2 >= 0 && angle2 <= 25 && angle1 >= 35 && angle1 <= 55 && (kinectController.hands[1].fingertips[0].X < kinectController.hands[1].palm.X) && (kinectController.hands[1].fingertips[1].X < kinectController.hands[1].palm.X)))
                        {
                            Invoke(new Action(delegate { SignTextBox.Text = "K"; }));
                        }                        
                        else if ((angle1 >= 35 && angle1 <= 65 && angle2 >= 50 && angle2 <= 70 && ((kinectController.hands[1].fingertips[0].Y < kinectController.hands[1].palm.Y && kinectController.hands[1].fingertips[1].Y < kinectController.hands[1].palm.Y) || (kinectController.hands[1].fingertips[1].Y > kinectController.hands[1].palm.Y && kinectController.hands[1].fingertips[0].Y > kinectController.hands[1].palm.Y))) || (angle2 >= 35 && angle2 <= 65 && angle1 >= 50 && angle1 <= 70 && ((kinectController.hands[1].fingertips[0].Y < kinectController.hands[1].palm.Y && kinectController.hands[1].fingertips[1].Y < kinectController.hands[1].palm.Y) || (kinectController.hands[1].fingertips[1].Y > kinectController.hands[1].palm.Y && kinectController.hands[1].fingertips[0].Y > kinectController.hands[1].palm.Y))))
                        {
                            Invoke(new Action(delegate { SignTextBox.Text = "C"; }));
                        }
                        else if ((angle1 >= 11 && angle1 <= 30 && angle2 >= 70 && angle2 <= 90 && (kinectController.hands[1].fingertips[0].X < kinectController.hands[1].palm.X)) || (angle2 >= 11 && angle2 <= 30 && angle1 >= 70 && angle1 <= 90 && (kinectController.hands[1].fingertips[1].X < kinectController.hands[1].palm.X)))
                        {
                            Invoke(new Action(delegate { SignTextBox.Text = "L"; }));
                        }
                        else if ((angle1 >= 11 && angle1 <= 30 && angle2 >= 70 && angle2 <= 90 && (kinectController.hands[1].fingertips[0].X > kinectController.hands[1].palm.X)) || (angle2 >= 11 && angle2 <= 30 && angle1 >= 70 && angle1 <= 90 && (kinectController.hands[1].fingertips[1].X > kinectController.hands[1].palm.X)))
                        {
                            Invoke(new Action(delegate { SignTextBox.Text = "P"; }));
                        }
                        else if ((kinectController.hands[0].fingertips[0].X < kinectController.hands[0].palm.X) && (kinectController.hands[0].fingertips[1].X < kinectController.hands[0].palm.X))
                        {
                            Invoke(new Action(delegate { SignTextBox.Text = "2"; }));
                        }
                        /*
                        else if ((angle1 >= 11 && angle1 <= 20 && angle2 >= 76 && angle2 <= 90) || (angle2 >= 11 && angle2 <= 20 && angle1 >= 76 && angle1 <= 90))
                        {
                            Invoke(new Action(delegate { SignTextBox.Text = "L"; }));
                        }
                        else if ((angle1 >= 25 && angle1 <= 40 && angle2 >= 70 && angle2 <= 80) || (angle2 >= 25 && angle2 <= 40 && angle1 >= 70 && angle1 <= 80))
                        {
                            Invoke(new Action(delegate { SignTextBox.Text = "P"; }));
                        }*/

                        Console.WriteLine("Y diff finger" + (kinectController.hands[0].fingertips[0].Y - kinectController.hands[0].fingertips[1].Y));
                    }

                    else if (kinectController.hands[1].fingertips.Count == 3)
                    {
                        double angle1, angle2, angle3;
                        angle1 = angle_vertical_axis(kinectController.hands[1].fingertips[0].X, kinectController.hands[1].fingertips[0].Y);
                        angle2 = angle_vertical_axis(kinectController.hands[1].fingertips[1].X, kinectController.hands[1].fingertips[1].Y);
                        angle3 = angle_vertical_axis(kinectController.hands[1].fingertips[2].X, kinectController.hands[1].fingertips[2].Y);
                        Console.WriteLine("angle 1 : " + angle1.ToString() + " " + "angle 2 : " + angle2.ToString() + " angle 3 : " + angle3.ToString());
                        //Invoke(new Action(delegate { SentenceTextBox2.Text = "angle 1 : " + angle1.ToString() + " " + "angle 2 : " + angle2.ToString() + " angle 3 : " + angle3.ToString(); }));
                        if ((angle1 >= 0 && angle1 <= 10 && angle2 >= 65 && angle2 <= 90 && angle3 >= 30 && angle3 <= 55) || (angle1 >= 0 && angle1 <= 10 && angle3 >= 65 && angle3 <= 90 && angle2 >= 30 && angle2 <= 55) || (angle3 >= 0 && angle3 <= 10 && angle2 >= 65 && angle2 <= 90 && angle1 >= 30 && angle1 <= 55) || 
                            (angle2 >= 0 && angle2 <= 10 && angle1 >= 65 && angle1 <= 90 && angle3 >= 30 && angle3 <= 55) || (angle2 >= 0 && angle2 <= 10 && angle3 >= 65 && angle3 <= 90 && angle1 >= 30 && angle1 <= 55) || (angle3 >= 0 && angle3 <= 10 && angle1 >= 65 && angle1 <= 90 && angle2 >= 30 && angle2 <= 55))
                            {
                                Invoke(new Action(delegate { SignTextBox.Text = "3"; }));
                            }
                            else if ((angle1 >= 0 && angle1 <= 10 && angle2 >= 65 && angle2 <= 90 && angle3 >= 25 && angle3 <= 40) || (angle1 >= 0 && angle1 <= 10 && angle3 >= 65 && angle3 <= 90 && angle2 >= 20 && angle2 <= 40) || (angle3 >= 0 && angle3 <= 10 && angle2 >= 65 && angle2 <= 90 && angle1 >= 20 && angle1 <= 40) ||
                            (angle2 >= 0 && angle2 <= 10 && angle1 >= 65 && angle1 <= 90 && angle3 >= 25 && angle3 <= 40) || (angle2 >= 0 && angle2 <= 10 && angle3 >= 65 && angle3 <= 90 && angle1 >= 20 && angle1 <= 40) || (angle3 >= 0 && angle3 <= 10 && angle1 >= 65 && angle1 <= 90 && angle2 >= 20 && angle2 <= 40))
                            {
                                Invoke(new Action(delegate { SignTextBox.Text = "F"; }));
                            }
                            else if ((angle1 >= 0 && angle1 <= 10 && angle2 >= 20 && angle2 <= 35 && angle3 >= 25 && angle3 <= 40) || (angle1 >= 0 && angle1 <= 10 && angle3 >= 20 && angle3 <= 35 && angle2 >= 25 && angle2 <= 40) || (angle3 >= 0 && angle3 <= 10 && angle2 >= 20 && angle2 <= 35 && angle1 >= 25 && angle1 <= 40) ||
                            (angle2 >= 0 && angle2 <= 10 && angle1 >= 20 && angle1 <= 35 && angle3 >= 25 && angle3 <= 40) || (angle2 >= 0 && angle2 <= 10 && angle3 >= 20 && angle3 <= 35 && angle1 >= 25 && angle1 <= 40) || (angle3 >= 0 && angle3 <= 10 && angle1 >= 20 && angle1 <= 35 && angle2 >= 25 && angle2 <= 40))
                            {
                                Invoke(new Action(delegate { SignTextBox.Text = "W"; }));
                            }

                    }
                    else if (kinectController.hands[1].fingertips.Count == 4)
                    {
                        Invoke(new Action(delegate { SignTextBox.Text = "4"; }));
                    }
                }
            });
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void SignTextBox_TextChanged(object sender, EventArgs e)
        {

        }

        private void RGBPicture_Click(object sender, EventArgs e)
        {

        }

        private void DepthPicture_Click(object sender, EventArgs e)
        {

        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void Startbutton_Click_1(object sender, EventArgs e)
        {
            // int img_count = 0;
            // Do nothing after the Camera color image is ready
            kinectController.clearEventColorReady();

            // After the depth image is ready and the tracking done
            KinectSkeleton.afterReady a = drawDepthImage; 
            a = a + showNumberFingers;

            kinectController.setEventDepthReady(a);
        }

        private void SignTextBox_TextChanged_1(object sender, EventArgs e)
        {

        }

        private void RGBPicture_Click_1(object sender, EventArgs e)
        {

        }

        private void DepthPicture_Click_1(object sender, EventArgs e)
        {

        }
    }
}

