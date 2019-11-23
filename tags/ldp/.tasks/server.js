import {
	watch,
	series,
	parallel
} from "gulp"
import bSync from "browser-sync";
import {
	jsCore
} from "./core-js"
import {
	jsTask
} from "./script"
import {
	pugTask
} from "./html"
import {
	cssCore
} from "./core-css"
import {
	cssTask
} from "./css"
import {
	copyAssets
} from "./copy";
import {
	cleanAssets
} from "./clean";

const server = () => {
	bSync.init({
		notify: false,
		server: {
			baseDir: "_dist",
		},
		port: 8000
	})

	watch([
		"src/js/*.js"
	], {
		delay: 750
	}, series(jsTask));

	watch([
		"src/**.pug",
		"src/_components/**/**.pug"
	], {
		delay: 750
	}, series(pugTask));

	watch([
		"src/css/**/**.scss"
	], {
		delay: 750
	}, series(cssTask));

	watch([
		"src/assets/**/**.{svg,png,jpg,speg,gif,mp4,flv,avi}"
	], {
		delay: 750
	}, series(cleanAssets, copyAssets));


	watch([
		"vendors/**/**.css",
		"vendors/**/**.js",
		"config.json"
	], {
		delay: 750
	}, parallel(jsCore, cssCore));

	watch([
		"_dist"
	]).on("change", bSync.reload);
}

module.exports = {
	server
};