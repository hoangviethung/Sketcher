module.exports = (() => {
	function n(n) {
		return document.getElementById(n)
	}
	
	document.addEventListener('DOMContentLoaded', function () {
		let e = n('loading'), t = n('progress'), o = n('progstat'), r = document.images, i = 0,
			d = r.length;
		if (0 === d) return u();
		
		function a() {
			let n = 100 / d * (i += 1) << 0;
			if (t.style.width = n, o.innerHTML = n, i === d) return u()
		}
		
		function u() {
			e.style.opacity = 0, setTimeout(function () {
				e.style.display = 'none';
				let n = document.getElementById('loading');
				n.parentNode.removeChild(n)
			}, 1500)
		}
		
		for (let c = 0; c < d; c++) {
			let s = new Image;
			s.onload = a, s.onerror = a, s.src = r[c].src
		}
	}, !1)
})();