import {
	src,
	dest
} from "gulp";
import {
	readFileSync
} from "graceful-fs";

export const copyAssets = () => {
	return src("./src/assets/**/**.{svg,png,jpg,jpeg,gif,mp4,flv,avi}")
		.pipe(dest("_dist/assets"))
}

export const copyFonts = () => {
	let glob = JSON.parse(readFileSync("config.json"));
	let fontList = glob.vendor.font;
	return src(fontList, {
			allowEmpty: true
		})
		.pipe(dest("_dist/fonts"));
}

export const copyFavicon = () => {
	return src("src/assets/favicon.ico", {
			allowEmpty: true
		})
		.pipe(dest("_dist"));
}

module.exports = {
	copyAssets,
	copyFonts,
	copyFavicon,
};