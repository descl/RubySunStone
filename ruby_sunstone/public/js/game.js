var screenWith = 0
var screenHeight = 0

var game = {
    // Run on page load.
    "onload" : function () {
    if (!me.video.init("screen", screenWith, screenHeight, true, 'auto')) {
        alert("Your browser does not support HTML5 canvas.");
        return;
    }
    me.debug.renderHitBox = true;
    me.loader.onload = this.loaded.bind(this);
    me.loader.preload(game.resources);
    me.state.change(me.state.LOADING);

    
    },
    "loaded" : function () {
    me.state.set(me.state.PLAY, new game.PlayScreen());
     
    me.entityPool.add("mainPlayer", game.PlayerEntity);
             
    me.input.bindKey(me.input.KEY.LEFT,  "left");
    me.input.bindKey(me.input.KEY.RIGHT, "right");
    me.input.bindKey(me.input.KEY.UP, "up");
    me.input.bindKey(me.input.KEY.DOWN, "down");

    me.state.change(me.state.PLAY);
    }
}

game.resources = [
    {name: "meta_tiles",  type:"image", src: "/ruby_sunstone/public/tiles/meta_tiles.png"},
    {name: "tree",  type:"image", src: "/ruby_sunstone/public/tiles/tree.png"},
    {name: "patate",  type:"image", src: "/ruby_sunstone/public/tiles/patate.png"},
    {name: "patate2",  type:"image", src: "/ruby_sunstone/public/tiles/patate2.png"},
    {name: "patate3",  type:"image", src: "/ruby_sunstone/public/tiles/patate3.png"},
    {name: "mainPlayer", type: "image",  src: "/ruby_sunstone/public/tiles/mainPlayer.png"},
    {name: "leve0", type: "tmx", src: "./public/levels/demo-antibes.tmx"}]
game.PlayScreen = me.ScreenObject.extend({
 
    onResetEvent: function() {
        me.levelDirector.loadLevel("leve0");
    }
 
});


game.PlayerEntity = me.ObjectEntity.extend({
 
    init: function(x, y, settings) {
        this.parent(x, y, settings);
        this.gravity = 0;
        this.setVelocity(4, 4);
 
        me.game.viewport.follow(this.pos, me.game.viewport.AXIS.BOTH);
    },
 
    update: function() {
 
        if (me.input.isKeyPressed('left')) {
            this.flipX(true);
            this.vel.x -= this.accel.x * me.timer.tick;
        } else if (me.input.isKeyPressed('right')) {
            this.flipX(false);
            this.vel.x += this.accel.x * me.timer.tick;
        } else if (me.input.isKeyPressed('up')) {
            this.flipX(false);
            this.vel.y -= this.accel.y * me.timer.tick;
        } else if (me.input.isKeyPressed('down')) {
            this.flipX(true);
            this.vel.y += this.accel.y * me.timer.tick;
        } else {
            this.vel.x = 0;
            this.vel.y = 0;
        }
        this.updateMovement();
 
        if (this.vel.x!=0 || this.vel.y!=0) {
            this.parent();
            return true;
        }
        return false;
    }
 
});


window.onReady(function() {
    screenHeight = jQuery(window).height()
    screenWith = jQuery(window).width()
    game.onload();
});
 