/*
  Stockfish, a UCI chess playing engine derived from Glaurung 2.1
  Copyright (C) 2004-2008 Tord Romstad (Glaurung author)
  Copyright (C) 2008-2015 Marco Costalba, Joona Kiiski, Tord Romstad
  Copyright (C) 2015-2018 Marco Costalba, Joona Kiiski, Gary Linscott, Tord Romstad

  Stockfish is free software: you can redistribute it and/or modify
  it under the terms of the GNU General Public License as published by
  the Free Software Foundation, either version 3 of the License, or
  (at your option) any later version.

  Stockfish is distributed in the hope that it will be useful,
  but WITHOUT ANY WARRANTY; without even the implied warranty of
  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
  GNU General Public License for more details.

  You should have received a copy of the GNU General Public License
  along with this program.  If not, see <http://www.gnu.org/licenses/>.
*/

#ifndef SEIRAWAN_TIMEMAN_H_INCLUDED
#define SEIRAWAN_TIMEMAN_H_INCLUDED

#include "seirawan_misc.hpp"
#include "seirawan_search.hpp"
#include "seirawan_thread.hpp"
#include "../Platform.h"

/// The TimeManagement class computes the optimal time to think depending on
/// the maximum available time, the game move number and other parameters.

namespace Seirawan
{
    
    class TimeManagement {
    public:
        void init(Search::LimitsType& limits, Color us, int32_t ply);

        int32_t optimum() const {
            return optimumTime;
        }

        int32_t maximum() const {
            return maximumTime;
        }

        int32_t elapsed() const {
            // return int(Search::Limits.npmsec ? Threads.nodes_searched() : now() - startTime);
            return now() - startTime;
        }
        
        int64_t availableNodes; // When in 'nodes as time' mode
        
    private:
        TimePoint startTime;
        int32_t optimumTime;
        int32_t maximumTime;
    };
    
}

#endif // #ifndef TIMEMAN_H_INCLUDED
