import {
	src,
	dest
} from "gulp";
import concat from "gulp-concat";
import uglify from "gulp-uglify";
import {
	readFileSync
} from "graceful-fs";

export const jsCore = () => {
	let glob = JSON.parse(readFileSync("config.json"));
	let jsVendorList = glob.vendor.js;
	return src(jsVendorList, {
			allowEmpty: true
		})
		.pipe(concat("core.min.js"))
		.pipe(uglify())
		.pipe(dest("_dist/js"))
};

module.exports = jsCore;