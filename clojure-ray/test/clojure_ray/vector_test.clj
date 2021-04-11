(ns clojure-ray.vector-test  
  (:require [clojure.test :refer :all]
            [clojure-ray.vector :refer :all]))

(require '[clojure.math.numeric-tower :refer [sqrt]])


(deftest vector-add
  (testing "Vector addition"
    (is (= (add [1 2 3] [2 3 4]) [3 5 7]))
))

(deftest vector-sub
  (testing "Vector subtraction"
    (is (= (subtract [1 2 3] [2 3 4]) [-1 -1 -1]))))

(deftest vector-mult
  (testing "Vector mulitply by scalar"
    (is (= (mult [1 2 3] 5) [5 10 15]))))

(deftest vector-div
  (testing "Vector divide by scalar"
    (is (= (divide [10 15 20] 5) [2 3 4]))))

(def root3 (sqrt 3))

(deftest vector-magnitude
  (testing "Vector magnitude"
    (is (= (magnitude [1 1 1]) (sqrt 3)))))


(deftest vector-dot-product
  (testing "Vector dot product"
    (is (= (dot [1 2 3] [2 3 4]) 20))))

(deftest vector-cross-product
  (testing "Vector cross product"
    (is (= (cross [1 2 3] [2 3 4]) [-1 2 -1]))))

(def one-on-root3 (/ 1 root3))

(deftest vector-unit
  (testing "Vector scaled back to unit"
    (is (= (unit [1 1 1]) [one-on-root3 one-on-root3 one-on-root3]))))