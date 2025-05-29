
#include <iostream>
#include <vector>
#include <string>
#include <cmath>
#include <complex>
#include "headers.h"
Signal filter(Signal signal,double cutoff_hz) {
    Fourier transformata(signal);
     int M  = signal.samples.size();  // liczba próbek
    double fs = N;                   // globalne N = częst. próbkowania [Hz]
    double df = fs / M;              // odstęp między kolejnymi binami

    // 1) DFT
    

    // 2) zerowanie składowych |f| > cutoff_hz
    for (int k = 0; k < M; ++k) {
        // wyznacz rzeczywistą częstotliwość binu k:
        double freq = k * df;
        if (k >= M/2)             // biny 'użyteczne' w drugiej połowie to odpowiedniki negatywnych freq
            freq = (k - M) * df;  
        if (std::abs(freq) > cutoff_hz) {
            transformata.X[k] = {0.0, 0.0};
        }
    }Signal filtered_signal("Filtered " + signal.name, transformata);
    return (filtered_signal);
}