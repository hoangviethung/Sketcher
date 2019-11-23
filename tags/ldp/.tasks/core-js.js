import {
	src,
	dest,
} from 'gulp';
import concat from 'gulp-concat';
import uglify from 'gulp-uglify';
import sourcemap from 'gulp-sourcemaps';
import {
	readFileSync,
} from 'graceful-fs';

const jsCore = () => {
	let glob = JSON.parse(readFileSync('config.json'));
	let jsVendorList = glob.vendor.js;
	return src(jsVendorList, {
			allowEmpty: true,
		})
		.pipe(sourcemap.init())
		.pipe(concat('core.min.js'))
		.pipe(uglify())
		.pipe(sourcemap.write('.'))
		.pipe(dest('./_dist/js'))
};

module.exports = {
	jsCore
};