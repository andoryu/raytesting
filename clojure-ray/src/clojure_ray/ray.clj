(ns clojure-ray.ray)

(require '[mikera.vectorz.core :as vec])


(defn create
  "creates a ray data instance - as a hashmap"
  [origin direction]
  
  {:origin (vec/vec3 origin) :direction (vec/vec3 direction)})


(defn at
  "return point along ray path"
  [ray t]
  
  (vec/add (:origin ray)
              (vec/scale (:direction ray) t)))


(defn hit-sphere
  "test sphere hit"
  [centre radius ray]

  (def oc (vec/sub (:origin ray) centre))
  (def a (vec/dot (:direction ray) (:direction ray)))
  (def b (* 2.0 (vec/dot oc (:direction ray))))
  (def c (- (vec/dot oc oc) (* radius radius)))
  (def discriminant (- (* b b) (* 4 (* a c))))

  (if (> discriminant 0.0) true false))