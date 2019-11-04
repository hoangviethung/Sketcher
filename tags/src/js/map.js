const GGMapInit = () => {
	const mapSelector = document.querySelector('#map');
	const dealerLocatorList = document.querySelector('.dealer-locator-list .list');

	if (mapSelector) {
		let map, markers = [],
			itemClicked;
		let locations = locationsInput;
		const mapOption = {
			zoom: 12,
			styles: [{
					'featureType': 'administrative',
					'elementType': 'labels.text.fill',
					'stylers': [{
						'color': '#444444',
					}, ],
				},
				{
					'featureType': 'landscape',
					'elementType': 'all',
					'stylers': [{
						'color': '#f2f2f2',
					}, ],
				},
				{
					'featureType': 'poi',
					'elementType': 'all',
					'stylers': [{
						'visibility': 'off',
					}, ],
				},
				{
					'featureType': 'road',
					'elementType': 'all',
					'stylers': [{
							'saturation': -100,
						},
						{
							'lightness': 45,
						},
					],
				},
				{
					'featureType': 'road.highway',
					'elementType': 'all',
					'stylers': [{
						'visibility': 'simplified',
					}, ],
				},
				{
					'featureType': 'road.arterial',
					'elementType': 'labels.icon',
					'stylers': [{
						'visibility': 'off',
					}, ],
				},
				{
					'featureType': 'transit',
					'elementType': 'all',
					'stylers': [{
						'visibility': 'off',
					}, ],
				},
				{
					'featureType': 'water',
					'elementType': 'all',
					'stylers': [{
							'color': '#0c6db5',
						},
						{
							'visibility': 'on',
						},
					],
				},
			],
		};
		const infoWindow = new google.maps.InfoWindow();

		const addMarkers = () => {
			const bounds = new google.maps.LatLngBounds();
			locations.forEach((location, index) => {
				let locationLatLng = new google.maps.LatLng(location.lat, location.lng)
				let marker = new google.maps.Marker({
					map: map,
					title: location.name,
					position: locationLatLng,
					icon: './assets/icons/marker-icon.svg',
				});
				bounds.extend(marker.position);
				markers.push(marker);
				showInfoMarkerOnMap(marker, index);
			});

			map.fitBounds(bounds);
		};

		const showInfoMarkerOnMap = (marker, index) => {
			google.maps.event.addListener(marker, 'click', function() {
				infoWindow.setContent(`
					<h3>${locations[index].name}</h3>
					<p>${locations[index].address}</p>
					<p>${locations[index].phone}</p>
				`);
				itemClicked = index;
				infoWindow.open(map, marker);
				map.panTo(marker.getPosition());
				map.setZoom(12);
			})
		};

		const getLocationList = () => {
			if (dealerLocatorList) {
				dealerLocatorList.innerHTML = '';
				markers.forEach((marker, index) => {
					if (map.getBounds().contains(marker.getPosition())) {
						const newMarker = document.createElement('div');
						newMarker.classList.add('dealer-locator-item');
						newMarker.innerHTML = `
						<h3>${locations[index].name}</h3>
						<p>${locations[index].address}</p>
						<p>${locations[index].phone}</p>
					`;
						newMarker.setAttribute('marker-id', `${index}`);
						newMarker.addEventListener('click', () => {
							itemClicked = index;
							const markerIndex = newMarker.getAttribute('marker-id');
							google.maps.event.trigger(markers[markerIndex], 'click');
						});
						dealerLocatorList.appendChild(newMarker);
					}
				});
			}
		};

		const initialize = () => {
			map = new google.maps.Map(mapSelector, mapOption);

			addMarkers();

			let listener = google.maps.event.addListener(map, 'idle', () => {
				if (map.getZoom() > 12) {
					map.setZoom(12);
				}
				google.maps.event.removeListener(listener);
			});
		};
		
		google.maps.event.addListener(map, 'bounds_changed', getLocationList);
		google.maps.event.addDomListener(window, 'load', initialize);
	}
};

export default GGMapInit;