#include <iostream>
#include <matplot/matplot.h>
#include <filesystem>
#include "headers.h"

void plot_signal(Signal signal) {
    using namespace matplot;

    // jak nie ma folderu na ugotowane wykresy to tworzy
    std::filesystem::path report_path = std::filesystem::current_path().parent_path() / "raport";
    if (!std::filesystem::exists(report_path)) {
        std::filesystem::create_directory(report_path);
    }

    // Czas to wektor a pingwin to walec
    std::vector<double> t(signal.samples.size());
    double dt = T / signal.samples.size();
    for(size_t i = 0; i < t.size(); i++) {
        t[i] = i * dt;
    }
    //zakres zmian i margines
    //robi wykres
    xlim({0, T-1});//zakresy x i y
    ylim({-1.2, 1.2});
    plot(t, signal.samples);
    xlabel("Time [s]"); // etykiety osi x i y
    ylabel("Amplitude");
    title("Signal: " + signal.name);
    std::stringstream ss;
    ss << std::fixed << std::setprecision(1) << signal.f; // 2 miejsca po przecinku
    std::string filename = (("../raport/"+signal.name+" "+((signal.name!="sin"&&signal.name != "cos" &&signal.name != "kwadrat" &&signal.name != "pila")?"":(ss.str()+"hz") )+ "_signal.png"));
    std::cout << "Saving plot to: " << filename<< std::endl;
    save(filename);//wygodny zapis do raportu
    show();
}
void plot_fourier(Fourier fourier) {
    using namespace matplot;

    // upewnia się że jest folder na raport jak nie ma to tworzy
    std::filesystem::path report_path = std::filesystem::current_path().parent_path() / "raport";
    if (!std::filesystem::exists(report_path)) {
        std::filesystem::create_directory(report_path);
    }

    // wektor częstotliwości zależny od próbkowania
    size_t M  = fourier.X.size();  // liczba próbek
    double fs = N;         // częstotliwość próbkowania [Hz]
    double df = fs / M;            // odstęp między kolejnymi momentami czasu dyskretnego

    // wyświetlić tylko nad kreską
    size_t H = M/2 + 1;            // liczba binów w rfft
    std::vector<double> f(H), magnitude(H);

    for (size_t k = 0; k < H; ++k) {
        // oś częstotliwości w [Hz]
        f[k] = k * df;

        // amplituda – standardowo skalujemy przez 2/M, żeby mieć
        // rzeczywiste wartości z dziedziny czasu
        magnitude[k] = 2.0 * std::abs(fourier.X[k]) / M;

    }
    //rysowanie wykresów
    xlim({0, *std::max_element(f.begin(),f.end())* 1.2});
    ylim({0, *std::max_element(magnitude.begin(), magnitude.end()) * 1.2});
    plot(f, magnitude);
    xlabel("Frequency [Hz]");
    ylabel("Magnitude");
    title("Fourier Transform");
        
    std::string filename = (("../raport/Fourier_transform.png"));
    std::cout << "Saving plot to: " << filename << std::endl;
    save(filename);
    show();
}