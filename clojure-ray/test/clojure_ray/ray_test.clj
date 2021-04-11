(ns clojure-ray.ray-test  
  (:require [clojure.test :refer :all]
            [clojure-ray.ray :refer :all]
            [mikera.vectorz.core :as vec]))

(deftest ray-create
  (testing "create a ray container"
    (is (= (create [0 0 0] [1 1 1]) 
           {:origin  (vec/vec3 [0 0 0])
            :direction (vec/vec3 [1 1 1])}))))


(deftest ray-at
  (testing "create a ray container"

    (def ray (create [1 1 1] [2 2 2]))
    (is (= (at ray 2.0) (vec/vec3 [5.0 5.0 5.0])))))

(deftest ray-sphere-hit-test
  (testing "ray hit against a sphere"
    (def ray (create [0 0 0] [ 0 0 -1]))
    (def test-sphere (vec/vec3 [0 0 -1]))
    (is (= (hit-sphere test-sphere 0.1 ray) true))
    ))