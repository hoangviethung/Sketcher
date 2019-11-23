import {
	src,
	dest
} from "gulp";
import concat from "gulp-concat";
import cssnano from "cssnano";
import postcss from "gulp-postcss";
import cssSort from "css-declaration-sorter";
import autoprefixer from "autoprefixer";
import {
	readFileSync
} from "graceful-fs";

export const cssCore = () => {
	const glob = JSON.parse(readFileSync("config.json"));
	const cssVendorList = glob.vendor.css;
	return src(cssVendorList, {
			allowEmpty: true
		})
		.pipe(concat("core.min.css"))
		.pipe(postcss([
			autoprefixer({
				cascade: false
			}),
			cssSort({
				order: "concentric-css",
			}),
			cssnano(),
		]))
		.pipe(dest("_dist/css"))
}

module.exports = cssCore;