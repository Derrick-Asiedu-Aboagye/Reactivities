import { MapContainer, Popup, Marker, TileLayer } from 'react-leaflet';
import 'leaflet/dist/leaflet.css';
import { Icon } from 'leaflet';
import marketIconPng from 'leaflet/dist/images/marker-icon.png';

type Props = {
    position: [number, number],
    venue: string
};

export default function MapComponents({ position, venue }: Props) {
    return (
        <MapContainer center={position} zoom={13} scrollWheelZoom={false} style={{ height: '100%' }}>
            <TileLayer
                url="https://{s}.tile.openstreetmap.org/{z}/{x}/{y}.png"
            />
            <Marker position={position} icon={new Icon({iconUrl: marketIconPng})}>
                <Popup>
                    {venue}
                </Popup>
            </Marker>
        </MapContainer>
    )
}