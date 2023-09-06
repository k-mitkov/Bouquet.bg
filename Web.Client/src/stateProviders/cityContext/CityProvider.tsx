import { useEffect, useState } from "react";
import { City } from "../../resources";
import { Location } from "@/types/MapTypes";
import { locationLokeren } from "@/resources/mapConfig";
import { CityContext } from "./CityContex";
import { useCities } from "../cachedDataContext/DataContext";


export default function CityProvider(props: any) {
    const { cities, isCitiesLoaded } = useCities();
    const [city, setCity] = useState<City>(cities.find(c => c.name == 'Sofia')!);
    const [isCityLoaded, setIsCityLoaded] = useState<boolean>(false);

    const [location, setLocation] = useState<Location>(locationLokeren);
    const [isLocationLoaded, setIsLocationLoaded] = useState<boolean>(false);

    function calculateDistance(location1: Location, location2: Location): number {
        const earthRadius = 6371; // Radius of the Earth in kilometers
        const lat1 = location1.lat;
        const lng1 = location1.lng;
        const lat2 = location2.lat;
        const lng2 = location2.lng;

        const dLat = (lat2 - lat1) * (Math.PI / 180);
        const dLng = (lng2 - lng1) * (Math.PI / 180);

        const a =
            Math.sin(dLat / 2) * Math.sin(dLat / 2) +
            Math.cos(lat1 * (Math.PI / 180)) * Math.cos(lat2 * (Math.PI / 180)) *
            Math.sin(dLng / 2) * Math.sin(dLng / 2);

        const c = 2 * Math.atan2(Math.sqrt(a), Math.sqrt(1 - a));
        const distance = earthRadius * c;

        return distance;
    }

    function findNearestCity(location: Location, cities: City[]): City | null {
        let nearestCity: City | null = null;
        let shortestDistance: number | null = null;

        for (const city of cities) {
            const cityLocation: Location = {
                lat: city.latitude,
                lng: city.longitude,
            };

            const distance = calculateDistance(location, cityLocation);

            if (shortestDistance === null || distance < shortestDistance) {
                shortestDistance = distance;
                nearestCity = city;
            }
        }
        return nearestCity;
    }

    useEffect(() => {
        async function getLocation() {
            setIsLocationLoaded(false);
            try {
                navigator.geolocation.getCurrentPosition(
                    (position) => {
                        setLocation({ lat: position.coords.latitude, lng: position.coords.longitude });
                    },
                    () => undefined
                );

            } catch (err) {
            }
            finally {
                setIsLocationLoaded(true);
            }
        }

        async function getNearestCity() {
            setIsCityLoaded(false);

            if (isCitiesLoaded) {
                setCity(findNearestCity(location, cities)!);
                setIsCityLoaded(true);
            }

        }

        getLocation();
        getNearestCity();
    }, [isCitiesLoaded])

    return (
        <CityContext.Provider value={{
            city: city, isCityLoaded: isCityLoaded, location: location, isLocationLoaded: isLocationLoaded
        }}>
            {props.children}
        </CityContext.Provider>
    )
}