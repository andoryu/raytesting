using System;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.Numerics;

namespace csharp_ray
{

    public class RayTracer
    {


        private Bitmap image;
        private int image_width, image_height;
        private Vector3 sphere, red, white, blue;
        private Random rand;

        public RayTracer(int image_width)
        {
            this.image_width = image_width;

            sphere = new Vector3(0, 0, -1);
            red = new Vector3(1.0f, 0.0f, 0.0f);
            white = new Vector3(1.0f, 1.0f, 1.0f);
            blue = new Vector3(0.5f, 0.7f, 1.0f);


            rand = new Random();

        }

        private float Clamp(float value, float min, float max)
        {
            if (value < min) return min;
            if (value > max) return max;
            return value;
        }

        //Standard library uses colour components in the range 0-255, engine uses 0.0-1.0
        private Color Vec3ToColour(Vector3 c)
        {
            var r = (int)(Clamp(c.X, 0.0f, 0.999f) * 256);
            var g = (int)(Clamp(c.Y, 0.0f, 0.999f) * 256);
            var b = (int)(Clamp(c.Z, 0.0f, 0.999f) * 256);

            // var r = (int)Math.Floor(r_f >= 1.0 ? 255 : r_f * 256.0);
            // var g = (int)Math.Floor(g_f >= 1.0 ? 255 : g_f * 256.0);
            // var b = (int)Math.Floor(b_f >= 1.0 ? 255 : b_f * 256.0);

            return Color.FromArgb(255, r, g, b);
        }

        private float RandomFloat(float min, float max)
        {
            return (float)((rand.NextDouble() * (max-min)) + min);
        }

        private Vector3 RandomInUnitSphere()
        {
            while(true)
            {
                var p = new Vector3(RandomFloat(-1,1), RandomFloat(-1,1), RandomFloat(-1,1));
                if(p.LengthSquared() >=1) continue;
                return p;
            }
        }

        private Vector3 RayColour(Ray ray, IHittable world, int depth)
        {
            HitRecord rec = new();

            if(depth <= 0)
                return new Vector3(0,0,0);


            if(world.Hit(ray, 0.001f, float.PositiveInfinity, ref rec))
            {
                var target = rec.point + rec.normal + RandomInUnitSphere();
                return 0.5f * RayColour( new Ray(rec.point, target - rec.point), world, depth-1);
            }

            Vector3 unit_dir = Vector3.Normalize(ray.direction);
            var t = 0.5f * (unit_dir.Y + 1.0f);
            var vec_colour = (1.0f-t)*white + (t*blue);

            return vec_colour;
        }

        private float HitSphere(Vector3 centre, float radius, Ray ray)
        {
            var oc = ray.origin - centre;

            var a = ray.direction.LengthSquared();
            var half_b = Vector3.Dot(oc, ray.direction);
            var c = oc.LengthSquared() - (radius*radius);
            var discriminant = (half_b*half_b) - (a*c);
            if (discriminant < 0)
            {
                return -1.0f;
            }
            else
            {
                return (-half_b - (float)Math.Sqrt(discriminant)) / a;
            }
        }


        private void WritePixel(Bitmap image, int x, int y, Vector3 colour, int samples)
        {
            //scale the colour by the number of samples used for anti-aliasing
            var scaled = colour/(float)samples;

            //adjust for gamma
            scaled.X = (float)Math.Sqrt(scaled.X);
            scaled.Y = (float)Math.Sqrt(scaled.Y);
            scaled.Z = (float)Math.Sqrt(scaled.Z);

            //adjust to 24-bit RGD w/ Alpha
            image.SetPixel(x, y, Vec3ToColour(scaled));
        }

        public void render()
        {

            //Image
            float aspect_ratio = 16.0f/9.0f;
            image_height = (int)(image_width/aspect_ratio);

            image = new Bitmap(image_width, image_height);

            //anti-aliasing - kills run time
            int samples_per_pixel = 100;
            int max_depth = 50;


            //World
            var world = new HittableList();
            world.Add(new Sphere(new Vector3(0,0,-1), 0.5f));
            world.Add(new Sphere(new Vector3(0, -100.5f, -1), 100));

            //Camera
            var cam = new Camera();

            //Random


            var watch = new Stopwatch();
            watch.Start();

            for(var x=0; x<image_width; x++)
            {
                for(var y=0; y<image_height; y++)
                {
                    var pixel_colour = new Vector3(0,0,0);
                    for(var s=0; s<samples_per_pixel; s++)
                    {
                        var u = (float)(x+rand.NextDouble()) / (image_width-1);
                        var v = (float)(image_height-1-y + rand.NextDouble()) / (image_height-1);
                        var r = cam.GetRay(u,v);
                        pixel_colour += RayColour(r, world, max_depth);
                    }

                    WritePixel(image, x, y, pixel_colour, samples_per_pixel);

                }
            }
            watch.Stop();

            image.Save("output.png", ImageFormat.Png);

            image.Dispose();

            Console.WriteLine($"Render completed in {watch.ElapsedMilliseconds} ms.");

        }

    }

}