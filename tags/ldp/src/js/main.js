import Loading from '../../vendors/loading';

// getPage
// getBranch
// provinceId

const ajaxPaginationProduct = () => {
	$('body').on('click', '.block-product .pagination-list li', function(e) {
		e.preventDefault();
		const url = $(this).find('a').attr('href');
		$.ajax({
			url: url,
			type: 'GET',
			success: function(res) {
				$('.block-product').html($(res).html());
			}
		})
	})
}

const ajaxSelectProvince = () => {
	$('body').on('change', '#province-select', function() {
		const provinceId = $(this).val();
		const url = $(this).attr('data-url');
		$.ajax({
			url: url,
			data: {
				ProvinceId: provinceId
			},
			type: 'POST',
			success: function(res) {
				$('.block-branch').html($(res).html());
				const currentUrl = new URL(window.location.href);
				const search = window.location.search;
				const params = new URLSearchParams(search)
				params.set('provinceid', provinceId);
				currentUrl.search = params.toString();
				window.history.pushState(null, '', currentUrl.toString());
			}
		})
	})
}

const LandinPageSlider = () => {
	const interleaveOffset = 0.5;
	const swiperOptions = {
		loop: true,
		speed: 1100,
		grabCursor: true,
		watchSlidesProgress: true,
		mousewheelControl: true,
		keyboardControl: true,
		effect: 'fade',
		fadeEffect: {
			crossFade: true,
		},
		autoplay: {
			delay: 3200,
			disabledOnInteraction: false,
		},
		navigation: {
			nextEl: ".swiper-button-next",
			prevEl: ".swiper-button-prev"
		},
		on: {
			progress: function() {
				var swiper = this;
				for (var i = 0; i < swiper.slides.length; i++) {
					var slideProgress = swiper.slides[i].progress;
					var innerOffset = swiper.width * interleaveOffset;
					var innerTranslate = slideProgress * innerOffset;
					swiper.slides[i].querySelector(".slide-inner").style.transform =
						"translate3d(" + innerTranslate + "px, 0, 0)";
				}
			},
			touchStart: function() {
				var swiper = this;
				for (var i = 0; i < swiper.slides.length; i++) {
					swiper.slides[i].style.transition = "";
				}
			},
			setTransition: function(speed) {
				var swiper = this;
				for (var i = 0; i < swiper.slides.length; i++) {
					swiper.slides[i].style.transition = speed + "ms";
					swiper.slides[i].querySelector(".slide-inner").style.transition =
						speed + "ms";
				}
			}
		}
	};
	let swiper = new Swiper('.ldp-banner .swiper-container', swiperOptions)
}

const WoWJS = () => {
	const wow = new WOW({
		boxClass: 'wow', // animated element css class (default is wow)
		animateClass: 'animated', // animation css class (default is animated)
		offset: 0, // distance to the element when triggering the animation (default is 0)
		mobile: true, // trigger animations on mobile devices (default is true)
		live: true, // act on asynchronously loaded content (default is true)
		callback: function(box) {
			// the callback is fired every time an animation is started
			// the argument that is passed in is the DOM node being animated
		},
		scrollContainer: null, // optional scroll container selector, otherwise use window,
		resetAnimation: true, // reset animation on end (default is true)
	});
	wow.init();
}
// ==> Call functions here
document.addEventListener('DOMContentLoaded', () => {
	Loading();
	objectFitImages('.ofcv');
	objectFitImages('.ofct');
	ajaxPaginationProduct();
	ajaxSelectProvince();
	LandinPageSlider();
	WoWJS();
});