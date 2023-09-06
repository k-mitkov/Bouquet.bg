export type MapConfig = {
    avatar: string;
    maxZoom: number;
    minZoom: number;
    style: string;
    accessToken: string;
    title: string;
    type?: string;
  };
  
  export type Location = {
    lat: number;
    lng: number;
  };

  export interface Viewport {
    width: string | number;
    height: string | number;
    latitude: number;
    longitude: number;
    zoom: number;
}