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
				$('.block-product .container').html($(res).find('.container').html());
				$(window).scrollTop($('.block-product').offset().top)
			}
		});
	})
}

const ajaxPaginationBranch = () => {
	$('body').on('click', '.block-branch .pagination-list li', function(e) {
		e.preventDefault();
		const url = $(this).find('a').attr('href');
		$.ajax({
			url: url,
			type: 'GET',
			beforeSend: function(res) {
				$('.block-branch .branch-list').addClass('ajax-loading');
			},
			complete: function(res) {
				$('.block-branch .branch-list').removeClass('ajax-loading');
			},
			success: function(res) {
				$('.block-branch .branch-list').html($(res).html());
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
			beforeSend: function(res) {
				$('.block-branch .branch-list').addClass('ajax-loading');
			},
			complete: function(res) {
				$('.block-branch .branch-list').removeClass('ajax-loading');
			},
			success: function(res) {
				$('.block-branch .branch-list').html($(res).html());
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

const toggleGoTopButton = () => {
	let currentScroll = document.querySelector("body").clientHeight - window.innerHeight;
	if (window.scrollY > 700 || window.scrollY === currentScroll) {
		document.getElementById("go-top").style.display = "flex";
		setTimeout(() => {
			document.getElementById("go-top").classList.add("show");
		}, 0);
	} else {
		document.getElementById("go-top").style.display = "none";
		document.getElementById("go-top").classList.remove("show");
	}
}

const goTop = () => {
	let goTopButton = document.getElementById("go-top");
	goTopButton.addEventListener("click", () => {
		window.scrollTo({
			top: 0,
			behavior: "smooth"
		});
	});
}

const customFancybox = () => {
	$('[data-fancybox]').fancybox({
		hash: false,
	})
}

const WoWJS = () => {
	return new WOW({
		boxClass: 'wow', // animated element css class (default is wow)
		animateClass: 'animated', // animation css class (default is animated)
		offset: 250, // distance to the element when triggering the animation (default is 0)
		callback: function(box) {
			// the callback is fired every time an animation is started
			// the argument that is passed in is the DOM node being animated
		},
	}).init();
}

// CONTROL SVG
function SVG() {
	jQuery('img.svg').each(function() {
		var $img = jQuery(this);
		var imgID = $img.attr('id');
		var imgClass = $img.attr('class');
		var imgURL = $img.attr('src');

		jQuery.get(imgURL, function(data) {
			// Get the SVG tag, ignore the rest
			var $svg = jQuery(data).find('svg');

			// Add replaced image's ID to the new SVG
			if (typeof imgID !== 'undefined') {
				$svg = $svg.attr('id', imgID);
			}
			// Add replaced image's classes to the new SVG
			if (typeof imgClass !== 'undefined') {
				$svg = $svg.attr('class', imgClass + ' replaced-svg');
			}

			// Remove any invalid XML tags as per http://validator.w3.org
			$svg = $svg.removeAttr('xmlns:a');

			// Check if the viewport is set, if the viewport is not set the SVG wont't scale.
			if (!$svg.attr('viewBox') && $svg.attr('height') && $svg.attr('width')) {
				$svg.attr('viewBox', '0 0 ' + $svg.attr('height') + ' ' + $svg.attr('width'))
			}

			// Replace image with new SVG
			$img.replaceWith($svg);

		}, 'xml');
	});
}

// ACTIVE ITEM MENU BY URL
function activeMenuByUrl() {
	const url = window.location.pathname;
	const listLink = $('.header-nav .header-nav-item a');
	listLink.each(function() {
		let allHref = $(this).attr('href');

		if (url === (allHref)) {
			$(this).parents('.header-nav-item').addClass('active');
		}
	})
}

function checkHeader() {
	const namePage = $('.index-2');
	namePage.parents('main').siblings('header').addClass('new-header');
}
// ==> Call functions here
document.addEventListener('DOMContentLoaded', () => {
	SVG();
	objectFitImages('.ofcv');
	objectFitImages('.ofct');
	Loading();
	customFancybox();
	toggleGoTopButton();
	goTop();
	ajaxPaginationProduct();
	ajaxPaginationBranch();
	ajaxSelectProvince();
	LandinPageSlider();
	WoWJS();
	activeMenuByUrl();
	checkHeader();
});

window.addEventListener('scroll', () => {
	toggleGoTopButton();
})