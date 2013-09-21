x0 = 7.1188869
y0 = 43.5853901

x1 = 7.131
y1 = 43.5792285

slicesX = 300.0
slicesY = slicesX* (3/5.0)


  task :import do
    require 'pg'

    conn = PG.connect( dbname: 'gis', user: 'gisuser', password: 'something' )



    xIncrement = (x1-x0) / slicesX
    yIncrement = (y1-y0) / slicesY

    datas = ""
    Integer(slicesY).times do |incY|
      yCur = y0 + yIncrement*incY
      line = ""
      Integer(slicesX).times do |incX|
        xCur = x0 + xIncrement*incX
        #puts "#{yCur} - #{xCur} "

        kind = 0

        query =  "SELECT osm_id,waterway,highway,building FROM planet_osm_polygon WHERE ST_CONTAINS(way, ST_GeometryFromText('POINT(#{xCur} #{yCur})', 4326))"

        conn.exec(query) do |result|
          result.each do |row|
            if row.values_at('waterway') !=  nil && kind == 0
              kind = 2
            elsif row.values_at('highway') !=  nil
              kind = 1
            elsif row.values_at('building') !=  nil && kind == 0
              kind = 3
            end
          end
        end
        line +=  "#{kind} "

      end
      datas += "#{line}\n"
    end

    File.open(Rails.root.join("datas","map2.yml"), 'w') { |file| file.write(datas) }
  end


  task :importPOIs=> :environment do
    File.open(Rails.root.join("datas","antibes_palmiers.csv"), "rb").each_line do |line|
        palmierTab =  line.split(",")

        xIncrement = (x1-x0) / slicesX
        yIncrement = (y1-y0) / slicesY

        xPos = palmierTab[0].to_f
        yPos = palmierTab[1].to_f

        x = Integer((xPos - x0) / xIncrement).abs
        y = Integer(((yPos - y0) / yIncrement)).abs
        monPoi = Poi.new(:x => x, :y => y,:kind => "1")
        monPoi.lvl = palmierTab[7]
        if monPoi.lvl == nil
          monPoi.lvl = 0
        end
        monPoi.save

    end
  end

