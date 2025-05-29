#include <vector>
#include <complex>
#include <cmath>
#include "headers.h"
std::vector<double>correlation(const Signal& signal1, const Signal& signal2) {

    std::vector<double> result(2 * N*T + 1, 0.0);
    
    for (size_t lag = 0; lag < 2 * N*T - 1; ++lag) {
        for (size_t n = 0; n < N*T; ++n) {
            if (n + lag < N*T) {
                result[lag] += signal1.samples[n] * signal2.samples[n + lag];
            } else if (n + lag >= N*T && n + lag < 2 * N*T - 1) {
                result[lag] += signal1.samples[n] * signal2.samples[n + lag - N*T];
            }
        }
    }
    for (size_t lag = 0; lag < result.size(); ++lag) {
        result[lag] /= *std::max_element(result.begin(),result.end()); // Normalizacja
    }
    return result;
}