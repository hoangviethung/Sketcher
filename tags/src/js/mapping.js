class Mapping {
	mobileNode;
	desktopNode;
	mobileMethod;
	desktopMethod;
	bpListener;
	selectorNode;
	
	constructor(selector, option) {
		this.selector = selector;
		this.mobileMethod = option.mobileMethod;
		this.mobileNode = option.mobileNode;
		this.desktopMethod = option.desktopMethod;
		this.desktopNode = option.desktopNode;
		this.breakpoint = option.breakpoint ? option.breakpoint : 1025;
		this.selectorNode = document.querySelector(this.selector);
		this.watch();
	};
	
	method(destinationSelector) {
		const destinationNode = document.querySelector(destinationSelector);
		const _this = this;
		return {
			insertBefore() {
				destinationNode.parentNode.insertBefore(_this.selectorNode, destinationNode.previousSibling);
			},
			insertAfter() {
				destinationNode.parentNode.insertBefore(_this.selectorNode, destinationNode.nextSibling);
			},
			appendTo() {
				destinationNode.appendChild(_this.selectorNode);
			},
			prependTo() {
				destinationNode.prepend(_this.selectorNode);
			},
		};
	};
	
	run(method, destinationSelector) {
		if (method === 'insertBefore') {
			return this.method(destinationSelector).insertBefore();
		}
		if (method === 'insertAfter') {
			return this.method(destinationSelector).insertAfter();
		}
		if (method === 'appendTo') {
			return this.method(destinationSelector).appendTo();
		}
		if (method === 'prependTo') {
			return this.method(destinationSelector).prependTo();
		}
	};
	
	watch() {
		this.bpListener = window.matchMedia(`(min-width: ${this.breakpoint}px)`);
		const checkWindowSize = (bp) => {
			if (bp.matches) {
				this.run(this.desktopMethod, this.desktopNode);
			} else {
				this.run(this.mobileMethod, this.mobileNode);
			}
		};
		if (!this.selectorNode) {
			return (() => {
				console.log('Selector not found');
			})();
		}
		return (() => {
			checkWindowSize(this.bpListener);
			this.bpListener.addListener(checkWindowSize);
		})();
	};
}

export default Mapping;