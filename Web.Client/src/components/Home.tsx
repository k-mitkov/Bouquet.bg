import { useEffect, useState } from 'react';
import { useTitle } from '../hooks';
import { Bouquet, City, Color, Flower, FlowerShop, getBouquets, Response } from '../resources';
import { useTranslation } from 'react-i18next';
import { useToast } from './ui/use-toast';
import { showError } from '@/helpers/ErrorHelper';
import CitySelect from './inputs/CitySelect';
import { useCities } from '@/stateProviders/cachedDataContext/DataContext';
import { useCity } from '@/stateProviders/cityContext';
import { useNavigate } from 'react-router-dom';
import Bouquets from './bouquet/Bouquets';
import ColorSelect from './inputs/ColorsSelector';
import FlowerSelect from './inputs/FlowerSelector';

const Home = () => {
  const [bouquets, setBouquets] = useState<Bouquet[]>([]);
  const [filteredBouquets, setFilteredBouquets] = useState<Bouquet[]>(bouquets);
  const { t } = useTranslation();
  const { toast } = useToast();
  const { cities, colors, flowers } = useCities();
  const { city, isCityLoaded } = useCity();
  const [selectedCity, setSelectedCity] = useState<City | undefined>(city);
  const [selectedShop, setSelectedShop] = useState<FlowerShop | undefined>(undefined);
  const navigate = useNavigate();
  useTitle(t('Home'));

  const fetchBouquets = async () => {
    try {
      let response: Response<Bouquet[]>;
      response = await getBouquets(selectedCity?.id || city.id);
      const bouquets: Bouquet[] = response.data;
      setBouquets(bouquets); // Update the state with the fetched stationsObjects
      setFilteredBouquets(bouquets);
    } catch (error) {
      showError(error, toast, t);
      setBouquets([]);
      setFilteredBouquets([])
    }
  };

  useEffect(() => {
    fetchBouquets();
  }, [selectedCity]);

  useEffect(() => {
    setSelectedCity(city);
  }, [city]);

  function handleCitySelect(cityID: string) {
    if (cityID != undefined) {
      const foundCity = cities.find((c) => c.id === cityID)
      setSelectedCity(foundCity)
    } else {
      setSelectedCity(undefined)
    }
  }

  function handleColorsSelect(colors: Color[]) {
    if(colors.length == 0){
      setFilteredBouquets(bouquets);
    }else{
      setFilteredBouquets(bouquets.filter(b => b.colors.find(c => colors.find(s => s.id == c.id) != undefined) != undefined))
    }
  }

  function handleFlowersSelect(flowers: Flower[]) {
    if(flowers.length == 0){
      setFilteredBouquets(bouquets);
    }else{
      setFilteredBouquets(bouquets.filter(b => b.flowers.find(c => flowers.find(s => s.id == c.id) != undefined) != undefined))
    }
  }

  return (
    <>
      <div className='flex flex-col w-[100%] items-center justify-center space-y-2 my-2 p-2 bg-secondary-background dark:bg-secondary-backgroud-dark rounded-lg'>
        <div className='flex flex-col md:flex-row space-x-5 justify-center text-black dark:text-gray-300'>
          <span className='text-start font-bold'>{(bouquets != undefined ? bouquets.length : '?') + ' ' + t('bouquets in ') + t(selectedCity?.name || city.name)}</span>
          <CitySelect
            defaultCity={city}
            cities={cities}
            onChange={(event) => {
              handleCitySelect(event as string);
            }}
            className='md:z-10'></CitySelect>
          <ColorSelect
            colors={colors}
            onChange={(event) => {
              handleColorsSelect(event as Color[]);
            }}
            className='md:z-10'></ColorSelect>
          <FlowerSelect
            flowers={flowers}
            onChange={(event) => {
              handleFlowersSelect(event as Flower[]);
            }}
            className='md:z-10'></FlowerSelect>
        </div>
        <Bouquets bouquets={filteredBouquets}></Bouquets>
      </div>
    </>
  );
};

export default Home;
