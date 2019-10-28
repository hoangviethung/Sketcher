import mapApi from './map';

const checkScroll = () => {
	if (window.scrollY > 0) {
		document.querySelector('body').classList.add('scrolled');
	} else {
		document.querySelector('body').classList.remove('scrolled');
	}
};

const newProductSlider = () => {
	return new Swiper('.index-2 .swiper-container', {
		speed: 1200,
		loop: true,
		navigation: {
			prevEl: '.index-2 .swiper-nav .swiper-prev',
			nextEl: '.index-2 .swiper-nav .swiper-next'
		},
		slidesPerColumn: 2,
		slidesPerView: 3,
		breakpoints: {
			1025: {
				slidesPerView: 2.25,
				spaceBetween: 20,
				slidesPerColumn: 1,
			}
		},
	})
};

const categoryBannerSlider = () => {
	return new Swiper('.index-1 .category-banner .swiper-container', {
		slidesPerView: 1,
		speed: 1400,
		spaceBetween: 20,
		autoplay: {
			delay: 2200,
			disableOnInteraction: false,
		},
		loop: true,
		navigation: {
			prevEl: '.index-1 .category-banner .swiper-container .swiper-prev',
			nextEl: '.index-1 .category-banner .swiper-container .swiper-next',
		}
	})
};

const homeVideoSlider = () => {
	return new Swiper('.index-4 .swiper-container', {
		slidesPerView: 1,
		grabCursor: true,
		speed: 1200,
		spaceBetween: -250,
		loop: true,
		loopAdditionalSlides: 2,
		loopedSlides: 2,
		breakpoints: {
			1600: {
				spaceBetween: -200,
			},
			1360: {
				spaceBetween: -150,
			},
			1200: {
				spaceBetween: -100,
			}
		}
	})
};

const toggleGoTopButton = () => {
	let currentScroll = document.querySelector('body').clientHeight - window.innerHeight;
	if (window.scrollY > 700 || window.scrollY === currentScroll) {
		document.getElementById('go-top').style.display = 'flex';
		setTimeout(() => {
			document.getElementById('go-top').classList.add('show');
		}, 0);
	} else {
		document.getElementById('go-top').style.display = 'none';
		document.getElementById('go-top').classList.remove('show');
	}
};

const goTop = () => {
	let goTopButton = document.getElementById('go-top')
	goTopButton.addEventListener('click', () => {
		window.scrollTo({
			top: 0,
			behavior: 'smooth',
		})
	})
};

const setColorFilter = () => {
	let colorArray = Array.from(document.querySelectorAll('nav.color-list .item'));
	colorArray.forEach(item => {
		item.style.backgroundColor = item.getAttribute('data-bg');
	})
};

const setLinkSocialShare = () => {
	const listSocialIcon = Array.from(document.querySelectorAll('.social-share .social-list a'));
	listSocialIcon.forEach(item => {
		const urlEncoded = encodeURIComponent(window.location.href);
		const socialLink = item.getAttribute('href');
		const finalUrl = socialLink + urlEncoded;
		item.setAttribute('href', finalUrl);
	})
};

const productDetailSlider = () => {
	const smallimageSlider = new Swiper('.product-detail-slider .small-image .swiper-container', {
		init: false,
		slidesPerView: 5,
		direction: 'vertical',
		spaceBetween: 5,
		freeMode: true,
		freeModeMomentum: true,
		freeModeSticky: true,
		observer: true,
		observerParents: true,
		speed: 900,
		breakpoints: {
			1440: {
				slidesPerView: 4,
			},
			1200: {
				slidesPerView: 5,
				direction: "horizontal",
			}
		},
		on: {
			init: function () {
				if (window.innerWidth < 1199) {
					Array.from(document.querySelectorAll('.product-detail-slider .small-image .swiper-slide .imgbox')).forEach(item => {
						item.style.height = (item.clientWidth + 2) + 'px';
					})
				}
			},
			resize: function () {
				if (window.innerWidth < 1199) {
					Array.from(document.querySelectorAll('.product-detail-slider .small-image .swiper-slide .imgbox')).forEach(item => {
						item.style.height = item.clientWidth + 'px';
					})
				}
				smallimageSlider.update();
			}
		}
	});

	const bigImagesSlider = new Swiper('.product-detail-slider .big-image .swiper-container', {
		slidesPerView: 1,
		watchSlidesVisibility: true,
		observer: true,
		observerParents: true,
		speed: 900,
		thumbs: {
			swiper: smallimageSlider,
		},
		on: {
			init: function () {
				smallimageSlider.init();
			},
			resize: function () {
				bigImagesSlider.update();
			}
		}
	})
};

const sliderProductDetailRelative = () => {
	return new Swiper('.product-detail-relative .swiper-container', {
		slidesPerView: 4,
		spaceBetween: 20,
		speed: 900,
		autoplay: {
			delay: 3500,
			disableOnInteraction: false,
		},
		nopeek: true,
		onSlideChangeEnd: function (s) {
			if (s.slides.length === s.activeIndex + 1) s.swipeTo(0);
		},
		breakpoints: {
			1024: {
				slidesPerView: 3,
			},
			768: {
				slidesPerView: 2,
			}
		}
	})
};

const toggleFilter = () => {
	const toggleBtn = document.querySelector('.product-page-wrapper .filter-toggle');
	const filter = document.querySelector('.product-page-wrapper .filter-wrapper');
	if (toggleBtn) {
		toggleBtn.addEventListener('click', () => {
			console.log(1);
			filter.classList.toggle('active');
		})
	}
};

const sliderProductCollections = () => {
	const slider1 = new Swiper('.product-collection-1 .swiper-container', {
		slidesPerView: 4,
		speed: 1400,
		loop: true,
		spaceBetween: 20,
		autoplay: {
			delay: 3600,
			disableOnInteraction: false,
		},
		pagination: {
			el: '.product-collection-1 .swiper-container .swiper-pagination-custom',
			type: 'bullets',
			clickable: true,
		},
		navigation: {
			prevEl: '.product-collection-1 .swiper-prev',
			nextEl: '.product-collection-1 .swiper-next',
		},
		breakpoints: {
			1200: {
				slidesPerView: 3,
			},
			1025: {
				slidesPerView: 2,
			},
			768: {
				slidesPerView: 1,
			}
		}
	});
	const slider2 = new Swiper('.product-collection-2 .swiper-container', {
		slidesPerView: 4,
		speed: 1400,
		loop: true,
		spaceBetween: 20,
		autoplay: {
			delay: 3600,
			disableOnInteraction: false,
		},
		pagination: {
			el: '.product-collection-2 .swiper-container .swiper-pagination-custom',
			type: 'bullets',
			clickable: true,
		},
		navigation: {
			prevEl: '.product-collection-2 .swiper-prev',
			nextEl: '.product-collection-2 .swiper-next',
		},
		breakpoints: {
			1200: {
				slidesPerView: 3,
			},
			1025: {
				slidesPerView: 2,
				slidesPerGroup: 6,
				slidesPerColumn: 2,
			},
			768: {
				slidesPerView: 2,
				slidesPerGroup: 4,
				slidesPerColumn: 2,
			}
		}
	});
	const slider3 = new Swiper('.product-collection-3 .swiper-container', {
		slidesPerView: 4,
		speed: 1400,
		loop: true,
		spaceBetween: 20,
		autoplay: {
			delay: 3600,
			disableOnInteraction: false,
		},
		pagination: {
			el: '.product-collection-3 .swiper-container .swiper-pagination-custom',
			type: 'bullets',
			clickable: true,
		},
		navigation: {
			prevEl: '.product-collection-3 .swiper-prev',
			nextEl: '.product-collection-3 .swiper-next',
		},
		breakpoints: {
			1200: {
				slidesPerView: 3,
			},
			1025: {
				slidesPerView: 2,
				slidesPerGroup: 6,
				slidesPerColumn: 2,
			},
			768: {
				slidesPerView: 2,
				slidesPerGroup: 4,
				slidesPerColumn: 2,
			}
		}
	});
	const slider4 = new Swiper('.product-collection-4 .swiper-container', {
		slidesPerView: 4,
		speed: 1400,
		loop: true,
		spaceBetween: 20,
		autoplay: {
			delay: 3600,
			disableOnInteraction: false,
		},
		pagination: {
			el: '.product-collection-4 .swiper-container .swiper-pagination-custom',
			type: 'bullets',
			clickable: true,
		},
		navigation: {
			prevEl: '.product-collection-4 .swiper-prev',
			nextEl: '.product-collection-4 .swiper-next',
		},
		breakpoints: {
			1200: {
				slidesPerView: 3,
			},
			1025: {
				slidesPerView: 2,
			},
			768: {
				slidesPerView: 1,
			}
		}
	});
	const slider5 = new Swiper('.product-collection-5 .swiper-container', {
		slidesPerView: 4,
		speed: 1400,
		loop: true,
		spaceBetween: 20,
		autoplay: {
			delay: 3600,
			disableOnInteraction: false,
		},
		pagination: {
			el: '.product-collection-5 .swiper-container .swiper-pagination-custom',
			type: 'bullets',
			clickable: true,
		},
		navigation: {
			prevEl: '.product-collection-5 .swiper-prev',
			nextEl: '.product-collection-5 .swiper-next',
		},
		breakpoints: {
			1200: {
				slidesPerView: 3,
			},
			1025: {
				slidesPerView: 2,
				slidesPerGroup: 6,
				slidesPerColumn: 2,
			},
			768: {
				slidesPerView: 2,
				slidesPerGroup: 4,
				slidesPerColumn: 2,
			}
		}
	});
};

// ==> Call functions here
document.addEventListener("DOMContentLoaded", () => {
	objectFitImages('.obj-fit-cover');
	mapApi();
	checkScroll();
	toggleGoTopButton();
	// homeBanner();
	newProductSlider();
	categoryBannerSlider();
	homeVideoSlider();
	goTop();
	setColorFilter();
	productDetailSlider();
	sliderProductDetailRelative();
	toggleFilter();
	sliderProductCollections();
	setLinkSocialShare();
});

window.addEventListener('scroll', () => {
	checkScroll();
	toggleGoTopButton();
});