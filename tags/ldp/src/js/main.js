import Loading from '../../vendors/loading';

// ==> Call functions here
document.addEventListener('DOMContentLoaded', () => {
	Loading();
	objectFitImages('.ofcv');
	objectFitImages('.ofct');
	ajaxPaginationProduct();
	ajaxSelectProvince();
});

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