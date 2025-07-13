import { PrismaClient } from '@prisma/client';

const prisma = new PrismaClient();

export const getUsers = async () => {
  return await prisma.user.findMany();
};
