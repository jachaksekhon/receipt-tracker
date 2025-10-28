import { PrismaClient, User } from '@prisma/client';

const prisma = new PrismaClient();

type NewUserInput = Pick<User, 'firstname' | 'lastname' | 'email' | 'password'>;

const userService = {
  getUsers: async () => {
    return prisma.user.findMany();
  },

  createUser: async (data: NewUserInput) => {
    return prisma.user.create({ data });
  },

  findUserByEmail: async (email: string) => {
    return prisma.user.findUnique({ where: { email } });
  }
};

export default userService

