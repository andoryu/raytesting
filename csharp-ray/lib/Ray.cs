using System.Numerics;

namespace csharp_ray
{
    public class Ray 
    {
        public Vector3 origin {get;}
        public Vector3 direction {get;}

        public Ray() 
        {
            origin =  new Vector3(0);
            direction = new Vector3(0);
        }
        public Ray(Vector3 origin, Vector3 direction)
        {
            this.origin =  origin;
            this.direction = direction;
        }

        public Vector3 at(float t) {
            return origin + (direction * t);
        }

    }
}