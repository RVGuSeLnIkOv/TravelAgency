using AutoMapper;
using TravelAgencyApi.Dto;
using TravelAgencyApi.Models;

namespace TravelAgencyApi.Helper
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles() 
        {
            CreateMap<Tourist, TouristDto>();
            CreateMap<TouristDto, Tourist>();
            CreateMap<TouristData, TouristDataDto>();
            CreateMap<TouristDataDto, TouristData>();
            CreateMap<Employee, EmployeeDto>();
            CreateMap<EmployeeDto, Employee>();
            CreateMap<EmployeeData, EmployeeDataDto>();
            CreateMap<EmployeeDataDto, EmployeeData>();
            CreateMap<Country, CountryDto>();
            CreateMap<CountryDto, Country>();
            CreateMap<City, CityDto>();
            CreateMap<CityDto, City>();
            CreateMap<Residence, ResidenceDto>();
            CreateMap<ResidenceDto, Residence>();
            CreateMap<TourOperator, TourOperatorDto>();
            CreateMap<TourOperatorDto, TourOperator>();
            CreateMap<TypeMeal, TypeMealDto>();
            CreateMap<TypeMealDto, TypeMeal>();
            CreateMap<Tour, TourDto>();
            CreateMap<TourDto, Tour>();
            CreateMap<Booking, BookingDto>();
            CreateMap<BookingDto, Booking>();
            CreateMap<TouristTour, TouristTourDto>();
            CreateMap<TouristTourDto, TouristTour>();
        }
    }
}
