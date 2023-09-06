import { useState } from 'react';
import { Marker, Popup, ViewState } from 'react-map-gl';
import markerImage from '../../resources/images/point.png';
import { FlowerShop } from '../../resources';
import { useNavigate } from 'react-router-dom';
import { useTranslation } from 'react-i18next';
import { Button } from '../shared/Button';

interface MarkerObject extends Partial<FlowerShop> {
  count?: number;
}

interface MapMarkerProps {
  flowerShops: FlowerShop[];
  zoomLevel: number;
  onViewportChange: (newViewport: ViewState) => void;
}

const MapMarker: React.FC<MapMarkerProps> = ({ flowerShops: electricStations, zoomLevel, onViewportChange }) => {
  const { t } = useTranslation();
  const navigate = useNavigate();

  const [selectedShop, setSelectedShop] = useState<MarkerObject | null>(null);

  const handleMarkerClick = (station: MarkerObject) => {
    if (station.count && station.count > 1) {
      // Merged marker behavior: Zoom the map and move the position
      onViewportChange({
        latitude: station.latitude || 0,
        longitude: station.longitude || 0,
        zoom: zoomLevel + 3
      });
    } else {
      // Single marker behavior: Show the popup
      setSelectedShop(station);
    }
  };

  const handleRouteClick = (station: MarkerObject) => {
    const { latitude, longitude } = station;
    const mapsUrl = `https://www.google.com/maps/dir/?api=1&destination=${latitude},${longitude}`;
    window.open(mapsUrl, '_blank');
  };

  const handleDetailsClick = (sObject: MarkerObject) => {
    navigate('/shop', { state: { sObject } });
  };

  const getProximityThreshold = (): number => {
    if (zoomLevel <= 8) {
      return 0.12; // Adjust this threshold based on your desired proximity at lower zoom levels
    } else if (zoomLevel <= 10) {
      return 0.025; // Adjust this threshold based on your desired proximity at lower zoom levels
    } else if (zoomLevel <= 14) {
      return 0.01; // Adjust this threshold based on your desired proximity at medium zoom levels
    } else {
      return 0.001; // Adjust this threshold based on your desired proximity at higher zoom levels
    }
  };

  const getMarkersWithCount = (): MarkerObject[] => {
    const proximityThreshold = getProximityThreshold();
    const mergedMarkers: MarkerObject[] = [];

    electricStations.forEach((station) => {
      const existingMarker = mergedMarkers.find((marker) => {
        const latDiff = Math.abs((marker.latitude || 0) - station.latitude);
        const lngDiff = Math.abs((marker.longitude || 0) - station.longitude);
        return latDiff <= proximityThreshold && lngDiff <= proximityThreshold;
      });

      if (existingMarker) {
        existingMarker.count = (existingMarker.count || 1) + 1;
        existingMarker.latitude = ((existingMarker.latitude || 0) + station.latitude) / 2; // Update latitude
        existingMarker.longitude = ((existingMarker.longitude || 0) + station.longitude) / 2; // Update longitude
      } else {
        mergedMarkers.push({ ...station });
      }
    });

    return mergedMarkers;
  };

  const markersWithCount = getMarkersWithCount();

  return (
    <>
      {markersWithCount.map((marker) => (
        <Marker key={`${marker.latitude}-${marker.longitude}`} latitude={marker.latitude || 0} longitude={marker.longitude || 0}>
          <div style={{ transform: 'translate(-50%, -100%)' }} onClick={() => handleMarkerClick(marker)}>
            <img src={markerImage} alt="Marker" className="w-8 h-8" />
            {marker.count && marker.count > 1 && (
              <div className="absolute top-1/2 left-1/2 transform -translate-x-1/2 -translate-y-1/2 text-black font-bold">
                {marker.count}
              </div>
            )}
          </div>
        </Marker>
      ))}

      {selectedShop && (
        <Popup
          className='flex flex-col p-10 min-2-[200px] rounded-lg text-black dark:text-gray-300'
          latitude={selectedShop.latitude || 0}
          longitude={selectedShop.longitude || 0}
          onClose={() => setSelectedShop(null)}
          closeButton={true}
          anchor="bottom"
        >
          <div className="flex flex-col bg-secondary-background dark:bg-secondary-backgroud-dark space-y-2 rounded-lg">
            {selectedShop.pictureDataUrl &&
              <img className='w-60 h-40 rounded-lg' src={import.meta.env.VITE_IDENTITY_FILE_URL + selectedShop.pictureDataUrl} />
            }
            <h3 className='text-center'>{selectedShop.name}</h3>
            <div className='flex flex-row justify-center space-x-5 p-2'>
            <Button type="submit" onClick={() => handleDetailsClick(selectedShop)}>{t('Details')}</Button>
            <Button type="submit" onClick={() => handleRouteClick(selectedShop)}>{t('Navigate to')}</Button>
            </div>
          </div>
        </Popup>
      )}
    </>
  );
};

export default MapMarker;