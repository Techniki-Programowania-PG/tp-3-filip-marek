#include <iostream>
#include <vector>
#include <string>
#include <cmath>
#include <complex>
#include "headers.h"

std::vector<double> addsignals(Signal signal1, Signal signal2) {// Funkcja do dodawania dwóch sygnałów
    
    std::vector<double> result(signal1.samples.size());
    for (size_t i = 0; i < signal1.samples.size(); ++i) {
        result[i] = signal1.samples[i] + signal2.samples[i];
    }
    for (size_t i = 0; i < signal1.samples.size(); ++i) {
        result[i] /=(*std::max_element(result.begin(), result.end()));
    }
    return result;
}