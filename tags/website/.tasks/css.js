import {
	src,
	dest
} from "gulp";
import sass from "gulp-sass";
import cssnano from "cssnano";
import rename from "gulp-rename";
import postcss from "gulp-postcss";
import autoprefixer from "autoprefixer";
import sourcemap from "gulp-sourcemaps";
import cssSort from "css-declaration-sorter";

export const cssTask = () => {
	return src(["src/css/**.scss"])
		.pipe(sourcemap.init())
		.pipe(sass.sync().on('error', sass.logError))
		.pipe(postcss([
			autoprefixer({
				cascade: false
			}),
			cssSort({
				order: "concentric-css",
			}),
			cssnano(),
		]))
		.pipe(rename({
			suffix: ".min"
		}))
		.pipe(sourcemap.write("."))
		.pipe(dest("_dist/css"))
}

module.exports = cssTask;