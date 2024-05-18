interface ApiResponse {
  success: boolean;
  message: string;
  responseCode: number;
}

export interface PersonAstronaut {
  personId: number;
  name: string;
  currentRank: string;
  currentDutyTitle: string;
  careerStartDate: Date;
  careerEndDate: Date;
}

export interface GetPersonResponse extends ApiResponse {
  person: PersonAstronaut;
}

export interface GetPeopleResponse extends ApiResponse {
  people: PersonAstronaut[];
}

export interface AddPersonResponse extends ApiResponse {
  id: number;
}

export interface AstronautDuty {
  id: number;
  personId: number;
  rank: string;
  dutyTitle: string;
  dutyStartDate: Date;
  dutyEndDate: Date;
}

export interface GetAstronautDutiesResponse extends ApiResponse {
  person: PersonAstronaut;
  astronautDuties: AstronautDuty[];
}

export interface CreateAstronautDuty {
  name: string;
  rank: string;
  dutyTitle: string;
  dutyStartDate: Date;
}
