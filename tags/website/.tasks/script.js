import {
	src,
	dest
} from "gulp";
// import babel from "gulp-babel";
import rename from "gulp-rename";
import plumber from "gulp-plumber";
import uglifyBabel from "gulp-terser";
import sourcemap from "gulp-sourcemaps";
import babelify from "babelify";
import browserify from "browserify";
import buffer from "vinyl-buffer";
import source from "vinyl-source-stream";

export const jsTask = () => {
	return browserify({
			basedir: '.',
			entries: ['src/js/main.js'],
			debug: true,
			sourceMaps: true
		})
		.transform(babelify, {
			presets: ["@babel/preset-env"]
		})
		.bundle()
		.pipe(source('main.js'))
		.pipe(buffer())
		.pipe(plumber(function(err) {
			console.log("========= ERROR! =========");
			console.log(err);
			console.log("=========================");
			this.emit('end');
		}))
		.pipe(sourcemap.init({
			loadMaps: true
		}))
		.pipe(uglifyBabel({
			mangle: false,
		}))
		.pipe(rename({
			suffix: ".min"
		}))
		.pipe(sourcemap.write('./'))
		.pipe(dest('./_dist/js'));
}

module.exports = jsTask;