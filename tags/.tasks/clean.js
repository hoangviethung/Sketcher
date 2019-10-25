import del from "del";

export const cleanDist = () => {
	return del("_dist")
}

export const cleanAssets = () => {
	return del("_dist/assets")
}

module.exports = {
	cleanDist,
	cleanAssets
};