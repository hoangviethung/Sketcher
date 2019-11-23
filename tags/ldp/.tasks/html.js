import {
	src,
	dest,
} from 'gulp';
import pug from 'gulp-pug';
import plumber from 'gulp-plumber';
import {
	readFileSync,
} from 'graceful-fs';

const pugTask = () => {
	const gulpConfig = JSON.parse(readFileSync('config.json'));
	let glob;
	if (gulpConfig.build) {
		glob = './src/**.pug';
	} else {
		glob = gulpConfig.filePath;
	}
	return src(glob)
		.pipe(plumber(function(err) {
			console.log(err);
			this.emit('end');
		}))
		.pipe(pug({
			pretty: '\t',
		}))
		.pipe(dest('./_dist'));
};

module.exports = {
	pugTask
};