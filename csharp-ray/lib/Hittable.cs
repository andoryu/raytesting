using System;
using System.Collections.Generic;
using System.Numerics;

namespace csharp_ray
{
    public class HitRecord
    {
        public Vector3 point;
        public Vector3 normal;
        public float t;
        public bool front_face;

        public void SetFaceNormal(Ray ray, Vector3 outward_normal)
        {
            front_face =Vector3.Dot(ray.direction, outward_normal) < 0;
            normal = front_face ? outward_normal : outward_normal;
        }
    }

    public interface IHittable
    {
        public bool Hit(Ray ray, float t_min, float t_max, ref HitRecord record);
    }

    public class HittableList : IHittable
    {
        public List<IHittable> object_list;

        public HittableList()
        {
            object_list = new();
        }

        public void Add(IHittable hittable_object)
        {
            object_list.Add(hittable_object);
        }

        public void Clear()
        {
            object_list.Clear();
        }

        public bool Hit(Ray ray, float t_min, float t_max, ref HitRecord record)
        {
            var temp_rec = new HitRecord();
            bool hit_anything = false;
            var closest_so_far = t_max;

            foreach(var obj in object_list)
            {
                if(obj.Hit(ray, t_min, closest_so_far, ref temp_rec))
                {
                    hit_anything = true;
                    closest_so_far = temp_rec.t;
                    record = temp_rec;
                }
            }

            return hit_anything;
        }

    }

    public class Sphere : IHittable
    {
        Vector3 centre;
        float radius;

        public Sphere(Vector3 centre, float radius)
        {
            this.centre = centre;
            this.radius = radius;
        }

        public bool Hit(Ray ray, float t_min, float t_max, ref HitRecord record)
        {
            var oc = ray.origin - centre;

            var a = ray.direction.LengthSquared();
            var half_b = Vector3.Dot(oc, ray.direction);
            var c = oc.LengthSquared() - (radius*radius);

            var discriminant = (half_b*half_b) - (a*c);
            if (discriminant < 0) 
            {
                return false;
            }        

            var sqrtd = (float)Math.Sqrt(discriminant);
            var root = (-half_b - sqrtd) / a;

            if (root < t_min || root > t_max)
            {
                root = (-half_b + sqrtd) / a;
                if(root < t_min || t_max < root)
                {
                    return false;
                }
            }

            record.t = root;
            record.point = ray.at(root);
            Vector3 outward_normal = (record.point - centre) /radius;
            record.SetFaceNormal(ray, outward_normal);

            return true;

        }
    }
}