/**
 * @license Copyright (c) 2003-2014, CKSource - Frederico Knabben. All rights reserved.
 * For licensing, see LICENSE.md or http://ckeditor.com/license
 */

CKEDITOR.editorConfig = function( config ) {
	// Define changes to default configuration here. For example:
	config.language = 'vi';
    // config.uiColor = '#AADC6E';
	config.allowedContent = true;
	config.extraAllowedContent = 'p(*)[*]{*};div(*)[*]{*};li(*)[*]{*};ul(*)[*]{*}';
	CKEDITOR.dtd.$removeEmpty.i = 0;
	
	config.entities = false;
	config.entities_latin = false;
	config.ForceSimpleAmpersand = true;
};
