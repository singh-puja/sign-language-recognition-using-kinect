using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SLRtry2
{
    class HandCoordinates
    {
        public FingerCoordinates palm { get; set; }
        public List<FingerCoordinates> fingertips { get; set; }

        public Coordinates palm3D;
        public List<Coordinates> fingertips3D { get; set; }

        public List<FingerCoordinates> contour { get; set; }
        public List<FingerCoordinates> inside { get; set; }

        public FingerCoordinates leftUpperCorner { get; set; }
        public FingerCoordinates rightDownCorner { get; set; }

        public HandCoordinates() // Constructor of Hand Class. When Hand class is instantiated, it is automatically called and the default properties are set for various attributes.
        {
            palm = new FingerCoordinates(-1, -1); // Set to impossible coordinates before capturing data.

            fingertips = new List<FingerCoordinates>(5); // List to the 5 fingertips to be tracked.
            fingertips3D = new List<Coordinates>(5); // Basically is used to track the X,Y,Z coordinates of all the 5 fingertips. List has five elements each of which have 3 properites each.

            contour = new List<FingerCoordinates>(2000);
            inside = new List<FingerCoordinates>(6000);

            leftUpperCorner = new FingerCoordinates(int.MaxValue, int.MaxValue);
            rightDownCorner = new FingerCoordinates(int.MinValue, int.MinValue);
        }

        // Check if a point is inside the hand countour box
        public bool isPointInsideContainerBox(FingerCoordinates p) // Takes a point as an arg and returns if point is inside the hand contour box or not.
        {
            if (p.X < rightDownCorner.X && p.X > leftUpperCorner.X
                && p.Y > leftUpperCorner.Y && p.Y < rightDownCorner.Y) // Note that the origin is on the upper-left corner of the box.
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool isCircleInsideContainerBox(FingerCoordinates p, float r)
        {
            if (leftUpperCorner.X > p.X - r)
            {
                return false;
            }
            if (rightDownCorner.X < p.X + r)
            {
                return false;
            }
            if (leftUpperCorner.Y > p.Y - r)
            {
                return false;
            }
            if (rightDownCorner.Y < p.Y + r)
            {
                return false;
            }

            return true;
        }

        // Calculate the contour box of the hand if it possible
        public bool calculateContainerBox(float reduction = 0)
        {
            if (contour != null && contour.Count > 0)
            {
                for (int j = 0; j < contour.Count; ++j)
                {
                    if (leftUpperCorner.X > contour[j].X)
                        leftUpperCorner.X = contour[j].X;

                    if (rightDownCorner.X < contour[j].X)
                        rightDownCorner.X = contour[j].X;

                    if (leftUpperCorner.Y > contour[j].Y)
                        leftUpperCorner.Y = contour[j].Y;

                    if (rightDownCorner.Y < contour[j].Y)
                        rightDownCorner.Y = contour[j].Y;
                }

                int incX = (int)((rightDownCorner.X - leftUpperCorner.X) * (reduction / 2));
                int incY = (int)((rightDownCorner.Y - leftUpperCorner.Y) * (reduction / 2));
                FingerCoordinates inc = new FingerCoordinates(incX, incY);
                leftUpperCorner = leftUpperCorner + inc;
                rightDownCorner = rightDownCorner + inc;

                return true;
            }
            else
            {
                return false;
            }
        }

        // Check if the hand is open
        public bool isOpen()
        {
            if (fingertips.Count == 5)
                return true;
            else
                return false;
        }

        // Check if the hand is close
        public bool isClose()
        {
            if (fingertips.Count == 0)
                return true;
            else
                return false;
        }

        // Obtain the 3D normalized point and add it to the list of fingertips
        public void calculate3DPoints(int width, int height, int[] distance)
        {
            if (palm.X != -1 && palm.Y != -1)
                palm3D = transformTo3DCoord(palm, width, height, distance[palm.X * width + palm.Y]); // Calculate 3-D position of palm

            fingertips3D.Clear(); // Empty any unwanted data.
            for (int i = 0; i < fingertips.Count; ++i)
            {
                FingerCoordinates f = fingertips[i]; // Store the value of each element in f use it as parameter in transformTo3DCoord
                if (palm.X - fingertips[i].X < 20) //reduces the noise, limits fingertips to 20 pixel distance from center.
                {
                    fingertips3D.Add(transformTo3DCoord(f, width, height, distance[f.X * width + f.Y])); // Append the modified elements. The useful set of points are actually in fingertips3D
                }
            }
        }

        // Normalize in the interval [-1, 1] the given 3D point.
        // The Z value is in the inverval [-1, 0], being 0 the closest distance.
        private Coordinates transformTo3DCoord(FingerCoordinates p, int width, int height, int distance)
        {
            Coordinates v = new Coordinates();
            v.X = p.Y / (width * 1.0f) * 2 - 1;
            v.Y = (1 - p.X / (height * 1.0f)) * 2 - 1;
            v.Z = (distance - 400) / -7600.0f;
            return v;
        }
    }
}

