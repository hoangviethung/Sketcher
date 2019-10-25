const setHeightBannerVideo = () => {
	if ($(window).width() >= 1025) {
		$('.home-banner #player').height($(window).height() - 110)
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
	let colorArray = Array.from(document.querySelectorAll('.filter-wrapper .color-list .item'));
	colorArray.forEach(item => {
		item.style.backgroundColor = item.getAttribute('data-bg');
	})
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
});

window.addEventListener('scroll', () => {
	toggleGoTopButton();
});