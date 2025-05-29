cmake_minimum_required(VERSION 3.15)
project(techniki_programowania3 VERSION 1.0)
include(FetchContent)

# ustawić matplot potem fetch
set(MATPLOT_BUILD_EXAMPLES OFF CACHE BOOL "")
set(MATPLOT_BUILD_TESTS OFF CACHE BOOL "")
set(MATPLOTPP_BACKEND "gnuplot" CACHE STRING "")

# znajdź pytona, podaj wszystko czego nie ma w systemie
find_package(Python3 COMPONENTS Interpreter Development REQUIRED)
set(FETCHCONTENT_BASE_DIR ${CMAKE_BINARY_DIR}/lib)

FetchContent_Declare(
    matplotplusplus
    GIT_REPOSITORY https://github.com/alandefreitas/matplotplusplus.git
    GIT_TAG master
)

FetchContent_Declare(
    pybind11
    GIT_REPOSITORY https://github.com/pybind/pybind11.git
    GIT_TAG master
)

FetchContent_MakeAvailable(matplotplusplus pybind11)
# minimum c
set(CMAKE_CXX_STANDARD 17)
set(CMAKE_CXX_STANDARD_REQUIRED ON)

# moduł pybind
pybind11_add_module(example 
    src/main.cpp
    src/generator.cpp
    src/plot.cpp
    src/dft.cpp
    src/idft.cpp
    src/Signal.cpp
    src/Fourier.cpp
    src/addsignals.cpp
    src/filtering1d.cpp
    src/correlation.cpp
)

# podaje biblioteki
target_link_libraries(example 
    PRIVATE
        Matplot++::matplot
        Python3::Python
)


# ustawić katalogi wyjściowe
set(CMAKE_LIBRARY_OUTPUT_DIRECTORY ${CMAKE_BINARY_DIR}/lib)
set(CMAKE_LIBRARY_OUTPUT_DIRECTORY_DEBUG ${CMAKE_BINARY_DIR}/lib)
# dodać ostrzeżenia kompilatora
if(MSVC)
    target_compile_options(example PRIVATE /W4)
else()
    target_compile_options(example PRIVATE -Wall -Wextra)
endif()