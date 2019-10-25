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

// ==> Call functions here
document.addEventListener("DOMContentLoaded", () => {
	objectFitImages('.obj-fit-cover');
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
});

window.addEventListener('scroll', () => {
	toggleGoTopButton();
});