#include "headers.h"
#include <iostream>
#include <vector>
#include <string>

    
    Fourier::Fourier(const Signal& signal) {
        X = dft(signal);
    }

    Fourier::~Fourier() {
        std::cout << "Fourier destructor called for Fourier transform" << std::endl;
    }
