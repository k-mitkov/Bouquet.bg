import { useEffect, useState } from "react";
import { City, Color, Flower, getCities, getColors, getFlowers } from "../../resources";
import { DataContext } from "./DataContext";


export default function DataProvider(props: any) {
    const [cities, setCities] = useState<City[]>([]);
    const [isCitiesLoaded, setIsCitiesLoaded] = useState<boolean>(false);

    const [flowers, setFlowers] = useState<Flower[]>([]);
    const [isFlowersLoaded, setIsFlowersLoaded] = useState<boolean>(false);

    const [colors, setColors] = useState<Color[]>([]);
    const [isColorsLoaded, setIsColorsLoaded] = useState<boolean>(false);

    useEffect(() => {
        async function initCities() {
            setIsCitiesLoaded(false);
            try {
                const cities = await getCities();
                setCities(cities.data);
                setIsCitiesLoaded(true);

            } catch (err) {
            }
            finally {
            }
        }

        async function initFlowers() {
            setIsFlowersLoaded(false);
            try {
                const flowers = await getFlowers();
                setFlowers(flowers.data);

            } catch (err) {
            }
            finally {
                setIsFlowersLoaded(true);
            }
        }

        async function initColors() {
            setIsColorsLoaded(false);
            try {
                const colors = await getColors();
                setColors(colors.data);

            } catch (err) {
            }
            finally {
                setIsColorsLoaded(true);
            }
        }

        initCities();
        initFlowers();
        initColors();
    }, [])

    return (
        <DataContext.Provider value={{
            cities: cities, isCitiesLoaded: isCitiesLoaded, flowers: flowers, isFlowersLoaded: isFlowersLoaded, colors: colors, isColorsLoaded: isColorsLoaded
        }}>
            {props.children}
        </DataContext.Provider>
    )
}