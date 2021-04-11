(ns clojure-ray.thedump)

(require '[clojure-ray.vector :as vector])
(require '[clojure-ray.ray :as ray])


(def ^:const img-width  1920)
(def ^:const img-height 1080)

(def ^:const viewport-height 2.0)
(def ^:const viewport-width (* (/ img-width img-height) viewport-height))

(def ^:const focal-length 1.0)


(defn pixel-colour
  "Test pixel colour"
  [ray]

  (def unitdir (vector/unit (:direction ray)))
  (def t (+ 1.0 (* 0.5 (get unitdir 1))))
  (def colour (vector/add (vector/mult [1.0 1.0 1.0] (- 1.0 t))
                          (vector/mult [0.5 0.7 1.0] t)))
  (rgb (get colour 0) (get colour 1) (get colour 2)))



  (def origin [0.0 0.0 0.0])

(def horizontal [viewport-width 0.0 0.0])
(def vertical [0.0 viewport-height 0.0])

(def lower-left-corner (vector/subtract
                        origin
                        (vector/divide horizontal 2)
                        (vector/divide vertical 2)
                        [0.0 0.0 focal-length]))

  ;; render
  ;;   
(def bitmap (new-image img-width img-height))


  (doseq [j (range img-height)
          i (range img-width)]
    
    (let [u (double (/ i (- img-width 1)))
          v (double (/ j (- img-height 1)))
          tmp (mapv + lower-left-corner
                          (vector/mult horizontal u)
                          (vector/mult vertical v))
          ray (ray/create origin
                          (mapv - tmp origin))
          colour (pixel-colour ray)]

      (set-pixel bitmap i (- img-height 1 j) colour)))

  (save bitmap "output/test.png"))