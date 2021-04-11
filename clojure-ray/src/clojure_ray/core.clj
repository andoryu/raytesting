(ns clojure-ray.core
  (:gen-class))

(require '[mikera.image.core :as image])
(require '[mikera.image.colours :as colours])

(require '[mikera.vectorz.core :as vec])

(require '[clojure-ray.ray :as ray])

;; drawing settings
(def ^:const img-width  1920)
(def ^:const img-height 1080)

(def ^:const viewport-height 2.0)
(def ^:const viewport-width (* (/ img-width img-height) viewport-height))

(def ^:const focal-length 1.0)


;; oft used colour vectors
; white
(def gradient1 (vec/vec3 [1.0 1.0 1.0]))
; blue
(def gradient2 (vec/vec3 [0.5 0.7 1.0]))

;;
(def test-sphere (vec/vec3 [0 0 -1]))



(defn gradient
  "return a gradient"
  [ray]

  (def unitdir (vec/normalise (:direction ray)))

  (def t (+ 1.0 (* 0.5 (vec/get unitdir 1))))

  (def colour (vec/add (vec/scale gradient1 (- 1.0 t))
                       (vec/scale gradient2 t)))

  (colours/rgb (vec/mget colour 0) (vec/mget colour 1) (vec/mget colour 2)))


(defn pixel-colour
  "Test pixel colour"
  [ray]

  (if (ray/hit-sphere test-sphere 0.5 ray)
    (colours/rgb 1 0 0)
    (gradient ray)))


(defn -main
  "Raytracing stuffs"
  []

  (def origin (vec/vec3 [0.0 0.0 0.0]))

  (def horizontal (vec/vec3 [viewport-width 0.0 0.0]))
  (def vertical (vec/vec3 [0.0 viewport-height 0.0]))

  (def lower-left-corner (vec/sub (vec/sub (vec/sub origin
                                                    (vec/scale horizontal 0.5))
                                           (vec/scale vertical 0.5))
                                  (vec/vec3 [0.0 0.0 focal-length])))

  ;; render
  ;;   
  (def bitmap (image/new-image img-width img-height))

  (doseq [j (range img-height)
          i (range img-width)]

    (let [u (double (/ i (- img-width 1)))
          v (double (/ j (- img-height 1)))
          dir (vec/sub (vec/add lower-left-corner
                                (vec/add (vec/scale horizontal u)
                                         (vec/scale vertical v)))
                       origin)

          ray (ray/create origin dir)
          colour (pixel-colour ray)]

      (image/set-pixel bitmap i (- img-height 1 j) colour)))


  (image/save bitmap "output/test.png"))

