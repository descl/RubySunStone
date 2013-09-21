require 'yaml'


class MapsController < ApplicationController
  before_action :set_map, only: [:show, :edit, :update, :destroy]

  # GET /maps
  # GET /maps.json
  def index
    f =  File.open(Rails.root.join("datas","map2.yml"))
    globalMap = Array.new
    f.each_line {|line| globalMap <<  line.split(' ')}


    x0 = Integer(params["x0"])
    y0 = Integer(params["y0"])
    width = Integer(params["width"])
    height = Integer(params["height"])

    x1 = x0 + width
    y1 = y0 + height

    @map = Array.new
    @poisMap = Array.new
    globalMap[y0...y1].each do |line|
      lineZE = Array.new
      width.times do
        lineZE << 0
      end
      @poisMap << lineZE
      @map << line[x0...x1]
    end

    @pois = Array.new
    Poi.all.each do |poi|
      if poi.x == nil
        continue
      end
      if poi.y == nil
        continue
      end
      if poi.x > (x0+1) && poi.x < (x1-3) && poi.y < y1 && poi.y > (y0+3)
        level = Integer((poi.lvl)/5)
        puts level

        begin

          arbres = Array.new
          arbres[0] = [[202,203],[210,211],[218,219]]
          arbres[1] = [[204,205],[212,213],[220,221]]
          arbres[2] = [[206,207],[214,215],[222,223]]
          arbres[3] = [[208,209],[216,217],[224,225]]

          #@poisMap[poi.y][poi.x]   = 1 + level*5
          #@poisMap[poi.y][poi.x+1] = 2 + level*5
          #@poisMap[poi.y-1][poi.x] = 3 + level*5
          #@poisMap[poi.y-1][poi.x+1] = 4 + level*5
          #@poisMap[poi.y-2][poi.x] = 5 + level*5
          #@poisMap[poi.y-2][poi.x+1] = 6 + level*5

          @poisMap[poi.y-1][poi.x+1]   = arbres[level][0][1]

          @poisMap[poi.y][poi.x]     = arbres[level][0][0]
          @poisMap[poi.y][poi.x+2]   = arbres[level][1][1]
          @poisMap[poi.y+1][poi.x+1]   = arbres[level][1][0]
          @poisMap[poi.y+1][poi.x+3]   = arbres[level][2][1]
          @poisMap[poi.y+2][poi.x+2] = arbres[level][2][0]
        rescue
        end
        #@pois << poi
      end
    end




    respond_to do |format|
      format.json { render :json => @map }
      format.xml {}
      format.html {}
    end
  end

  # GET /maps/1
  # GET /maps/1.json
  def show
  end

  # GET /maps/new
  def new
    @map = Map.new
  end

  # GET /maps/1/edit
  def edit
  end

  # POST /maps
  # POST /maps.json
  def create
    @map = Map.new(map_params)

    respond_to do |format|
      if @map.save
        format.html { redirect_to @map, notice: 'Map was successfully created.' }
        format.json { render action: 'show', status: :created, location: @map }
      else
        format.html { render action: 'new' }
        format.json { render json: @map.errors, status: :unprocessable_entity }
      end
    end
  end

  # PATCH/PUT /maps/1
  # PATCH/PUT /maps/1.json
  def update
    respond_to do |format|
      if @map.update(map_params)
        format.html { redirect_to @map, notice: 'Map was successfully updated.' }
        format.json { head :no_content }
      else
        format.html { render action: 'edit' }
        format.json { render json: @map.errors, status: :unprocessable_entity }
      end
    end
  end

  # DELETE /maps/1
  # DELETE /maps/1.json
  def destroy
    @map.destroy
    respond_to do |format|
      format.html { redirect_to maps_url }
      format.json { head :no_content }
    end
  end

  private
    # Use callbacks to share common setup or constraints between actions.
    def set_map
      @map = Map.find(params[:id])
    end

    # Never trust parameters from the scary internet, only allow the white list through.
    def map_params
      params[:map]
    end
end
