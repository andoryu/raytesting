(ns clojure-ray.vector)

(require '[clojure.math.numeric-tower :refer [sqrt]])

(defn add
  "Add vectors together"
  [va vb]

  (mapv + va vb))


(defn subtract
  "subtract vectors"
  [va vb]

  (mapv - va vb))


(defn mult
  "Multiply a vector by a scalar"
  [v scalar]
  
  (mapv (fn [x] (* x scalar)) v))


(defn divide
  "Divide a vector by a scalar"
  [v scalar]
  
  (mapv (fn [x] (/ x scalar)) v))


(defn magnitude
  "Magnitude of a vector"
  [v]

  (sqrt
   (reduce +
           (mapv * v v)))
  
)


(defn dot
  "Vector dot product"
  [va vb]
  
  (reduce + (mapv * va vb)))

(defn cross
  "Vector cross product"
  [va vb]
  
  [
   (- (* (get va 1) (get vb 2)) (* (get va 2) (get vb 1)))
   (- (* (get va 2) (get vb 0)) (* (get va 0) (get vb 2)))
   (- (* (get va 0) (get vb 1)) (* (get va 1) (get vb 0)))
  ])


(defn unit
  "Make a vector unit length"
  [v]

  (divide v (magnitude v)))