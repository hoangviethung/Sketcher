import {
	series,
} from "gulp";

// Import tasks
import {
	server
} from "./.tasks/server";
import {
	jsTask
} from "./.tasks/script";
import {
	pugTask
} from "./.tasks/html";
import {
	cssTask
} from "./.tasks/css";
import {
	jsCore
} from "./.tasks/core-js";
import {
	cssCore
} from "./.tasks/core-css";
import {
	cleanDist
} from "./.tasks/clean";
import {
	copyFonts,
	copyFavicon,
	copyAssets
} from "./.tasks/copy";


exports.default = series(
	cleanDist,
	copyFavicon,
	copyFonts,
	copyAssets,
	jsCore,
	cssCore,
	cssTask,
	jsTask,
	pugTask,
	server,
)