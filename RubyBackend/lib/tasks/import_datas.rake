


  task :import do
    require 'pg'

    conn = PG.connect( dbname: 'gis', user: 'gisuser', password: 'something' )








    x0 = 7.1188869
    y0 = 43.5853901

    x1 = 7.131
    y1 = 43.5792285

    slices = 50.0

    xIncrement = (x1-x0) / slices
    yIncrement = (y1-y0) / slices

    datas = ""
    Integer(slices).times do |incY|
      yCur = y0 + yIncrement*incY
      line = ""
      Integer(slices).times do |incX|
        xCur = x0 + xIncrement*incX
        #puts "#{yCur} - #{xCur} "

        kind = 2

        query =  "SELECT osm_id,waterway,highway,building FROM planet_osm_polygon WHERE ST_CONTAINS(way, ST_GeometryFromText('POINT(#{xCur} #{yCur})', 4326))"

        conn.exec(query) do |result|
          result.each do |row|
            if row.values_at('waterway') !=  nil
              kind = 0
            elsif row.values_at('highway') !=  nil
              kind = 1
            elsif row.values_at('building') !=  nil
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

