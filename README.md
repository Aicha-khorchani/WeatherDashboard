# WeatherDashboard

## Overview
A WPF MVVM application to visualize weather information:
- Current weather (temperature, humidity, wind)
- 7-day forecast
- Daily temperature & humidity trends (OxyPlot)
- Weather icons
## API Provider

This project will be using the [Open-Meteo API](https://open-meteo.com/) to fetch weather data:
- Current weather
- Hourly forecasts
- Daily forecasts
- Temperature, humidity, wind, and weather codes (icons)

No API key is required.

## Features
- City search (geocoding API, no key required)
- Current weather panel
- Weekly forecast list
- Hourly plots for temperature & humidity
- Icons mapping for weather codes
- Error & loading handling

## Architecture
- MVVM pattern
- Views, ViewModels, Models
- Services for API
- Helpers for logging & notifications
- Assets folder for icons and styles

## Requirements
- .NET 7
- OxyPlot 2.2
- WPF
