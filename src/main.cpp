#include <iostream>
#include <matplot/matplot.h>
#include <pybind11/stl.h>
#include <pybind11/pybind11.h>
#include "headers.h"
#include<pybind11/complex.h>
//konfiguracja pybind11
PYBIND11_MODULE(example, m) { 
    m.doc() = "Example module using pybind11";

    

    // definicje do pybind 
    pybind11::class_<Signal>(m, "Signal")
        .def(pybind11::init<double, double, std::string>())
        .def(pybind11::init< std::string, Fourier>())
        .def(pybind11::init<Signal, Signal>())
        .def(pybind11::init<const std::vector<double>&>())
        .def_readwrite("frequency", &Signal::f)
        .def_readwrite("name", &Signal::name)
        .def_readwrite("samples", &Signal::samples);

    pybind11::class_<Fourier>(m, "Fourier")
        .def(pybind11::init<Signal>())
        .def_readwrite("X", &Fourier::X);

    m.attr("N")= N;  
    m.attr("PI") = PI;  
    m.def("plot_signal", &plot_signal, "Plot signal using matplot++",
        pybind11::arg("signal"));
    
    m.def("plot_fourier", &plot_fourier, "Plot Fourier transform using matplot++",
        pybind11::arg("fourier"));
          
    m.def("dft", &dft, "Discrete Fourier Transform",
        pybind11::arg("signal"));

    m.def("idft", &idft, "Inverse Discrete Fourier Transform",
        pybind11::arg("fourier"));
    m.def("addsignals", &addsignals, "Add two signals",
        pybind11::arg("signal1"), pybind11::arg("signal2"));
    m.def("filter", &filter, "Filter signal with a cutoff frequency",
        pybind11::arg("signal"), pybind11::arg("cutoff_hz"));
    m.def("correlation", &correlation, "Calculate correlation between two signals",
        pybind11::arg("signal1"), pybind11::arg("signal2"));
    
}
