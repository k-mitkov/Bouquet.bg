import React, { useEffect, useState } from 'react';
import ReactMapGL, { ViewState } from 'react-map-gl';
import { mapConfig } from "../../resources/mapConfig";
import { City, FlowerShop } from '../../resources';
import { Location } from "../../types/MapTypes";
import MapMarker from './MapMarker';
import { useSettings } from '../../stateProviders/settingsContext/SettingsContext';
import { useCity } from '@/stateProviders/cityContext';

interface MapProps {
  flowerShops: FlowerShop[];
  height: string;
  width: string;
  allowPointSelection?: boolean;
  onObjectAdded?: (station: FlowerShop) => void; // Callback to notify parent component about new object addition
  selectedCity?: City;
}

const Map: React.FC<MapProps> = ({ flowerShops: initialShops, height, width, allowPointSelection, onObjectAdded, selectedCity }) => {
  const [zoomLevel, setZoomLevel] = useState<number>(6.5);
  const { location } = useCity();
  const [latLng, setLatLng] = useState<Location>(location);
  const [hasOneStation, setHasOneStation] = useState<boolean>(false);
  const [shops, setShops] = useState<FlowerShop[]>(initialShops);
  const { theme } = useSettings();

  const mapStyle = theme === 'light'
    ? "mapbox://styles/mapbox/streets-v12"
    : "mapbox://styles/mapbox/navigation-night-v1";


  useEffect(() => {
    setHasOneStation(shops != undefined && shops.length === 1);
  }, [shops]);

  useEffect(() => {
    if (hasOneStation) {
      const station = shops[0];
      setLatLng({ lat: station.latitude, lng: station.longitude });
      setZoomLevel(12);
    } else if (selectedCity != undefined) {
      setLatLng({ lat: selectedCity.latitude, lng: selectedCity.longitude });
      setZoomLevel(10);
    } else if(shops.length === 0 && allowPointSelection) {
      setLatLng(location);
    }
  }, [hasOneStation, shops, allowPointSelection, selectedCity]);

  useEffect(() => {
    if (!allowPointSelection)
      setShops(initialShops);
  }, [initialShops]);

  const handleViewportChange = (newViewport: ViewState) => {
    const { latitude, longitude, zoom } = newViewport;
    setZoomLevel(zoom);
    setLatLng({ lat: latitude, lng: longitude });
  };

  const handleMapClick = (event: any) => {
    if (allowPointSelection) {
      const [longitude, latitude] = event.lngLat;
      const newStation: FlowerShop = {
        id: '',
        latitude: latitude,
        longitude: longitude,
        name: "New shop",
        address: '',
        pictureDataUrl: '',
        cityID: '',
        city: '',
        ownerId: '',
        workers: [],
        shopConfig: undefined,
      };

      // Notify the parent component about the new station addition
      onObjectAdded!(newStation);

      setShops([newStation]);
    }
  };

  const viewport: ViewState = {
    latitude: latLng.lat,
    longitude: latLng.lng,
    zoom: zoomLevel
  };

  return (
    <ReactMapGL className='rounded-lg'
      width={width}
      height={height}
      {...viewport}
      maxZoom={mapConfig.maxZoom}
      minZoom={mapConfig.minZoom}
      mapboxApiAccessToken={mapConfig.accessToken}
      mapStyle={mapStyle}
      onViewportChange={handleViewportChange}
      onClick={handleMapClick}
    >
      <MapMarker flowerShops={shops} zoomLevel={viewport.zoom} onViewportChange={handleViewportChange} />
    </ReactMapGL>
  );
};

export default Map;
